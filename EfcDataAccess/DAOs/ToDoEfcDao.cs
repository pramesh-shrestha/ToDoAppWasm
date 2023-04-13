using Application.DAOInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs; 

public class ToDoEfcDao : ITodoDao {

    private readonly ToDoContext context;

    public ToDoEfcDao(ToDoContext context) {
        this.context = context;
    }

    public async Task<Todo> CreateAsync(Todo todo) {
        EntityEntry<Todo> newTodo = await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return newTodo.Entity;
    }

    public async Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters) {
        //Include will include the Owner in todos
        IQueryable<Todo> query = context.Todos.Include(todo => todo.Owner).AsQueryable();
        if (!string.IsNullOrEmpty(searchParameters.Username)) {
            query = query.Where(todo => todo.Owner.UserName.ToLower().Equals(searchParameters.Username.ToLower()));
        }

        if (searchParameters.UserId != null) {
            query = query.Where(t => t.Owner.Id == searchParameters.UserId);
        }
        if (searchParameters.CompletedStatus != null) {
            query = query.Where(t => t.IsCompleted == searchParameters.CompletedStatus);
        }
        if (!string.IsNullOrEmpty(searchParameters.TitleContains)) {
            query = query.Where(t =>
                t.Title.ToLower().Contains(searchParameters.TitleContains.ToLower()));
        }

        List<Todo> result = await query.ToListAsync();
        return result;
    }

    public async Task UpdateAsync(Todo todo) {
        //In the FileContext, we would remove, then add a To-do.
        //The DbSet has an Update method, which will search for an existing object with the same Id, and just overwrite the data.
        // context.ChangeTracker.Clear();
        context.Todos.Update(todo);
        await context.SaveChangesAsync();
    }

    public async Task<Todo?> GetByIdAsync(int id) {
        //Here, we must also want to retrieve the data of the Owner which is a User
        Todo? existingTodo =
            await context.Todos
                .AsNoTracking()//We don't want the To-do to be kept in the ChangeTracker, when we fetch it the first time.
                .Include(todo => todo.Owner)
                .SingleOrDefaultAsync(todo => todo.Id == id);
        return existingTodo;
    }
    
    public async Task DeleteAsyncById(int id) {
        Todo? existingTodo = await GetByIdAsync(id);
        if (existingTodo == null) {
            throw new Exception($"Todo with id {id} not found");
        }

        context.Todos.Remove(existingTodo);
        await context.SaveChangesAsync();
    }
}