using Application.DAOInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;

namespace FileData.DAOs; 

public class TodoFileDao : ITodoDao {

    private readonly FileContext context;

    public TodoFileDao(FileContext context) {
        this.context = context;
    }

    public Task<Todo> CreateAsync(Todo todo) {
        int id = 1;
        //finding the last id that was given
        if (context.Todos.Any()) {
            id = context.Todos.Max(td => td.Id);
            id++;
        }

        todo.Id = id; //setting the id to 'todo' passed in as argument
        context.Todos.Add(todo);
        context.SaveChanges();
        return Task.FromResult(todo);
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters) {

        IEnumerable<Todo> todos = context.Todos.AsEnumerable();

        if (searchParameters.Username != null) {
            todos = context.Todos.Where(todo =>
                todo.Owner.UserName.Equals(searchParameters.Username, StringComparison.OrdinalIgnoreCase));
        }

        if (searchParameters.UserId != null) {
            todos = context.Todos.Where(todo =>
                todo.Id == searchParameters.UserId);
        }

        if (searchParameters.CompletedStatus != null) {
            todos = context.Todos.Where(todo =>
                todo.IsCompleted == searchParameters.CompletedStatus);
        }

        if (searchParameters.TitleContains != null) {
            todos = context.Todos.Where(todo =>
                todo.Title.Equals(searchParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(todos);
    }

    public Task UpdateAsync(Todo toUpdate) {
        Todo? existing = context.Todos.FirstOrDefault(todo => todo.Id == toUpdate.Id);
        if (existing == null) {
            throw new Exception($"Todo with id {toUpdate.Id} does not exist!");
        }

        context.Todos.Remove(existing);
        context.Todos.Add(toUpdate);
        context.SaveChanges();
        return Task.CompletedTask; //Task.Completed task is returned, because the return type is Task, and the method is not marked "async";
    }

    public Task<Todo?> GetByIdAsync(int id) {
        Todo? todo = context.Todos.FirstOrDefault(td => td.Id == id);
        return Task.FromResult(todo);   
    }

    //delete by dto
    public Task DeleteAsync(TodoDeleteDto dto) {

        Todo? existing = context.Todos.FirstOrDefault(td =>
            td.Id == dto.Id);
        if (existing == null) {
            throw new Exception($"Todo with id {dto.Id} does not exist!");
        }

        context.Todos.Remove(existing);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    //delete by id
    public Task DeleteAsyncById(int id) {
        Todo? existing = context.Todos.FirstOrDefault(td => td.Id == id);
        
        if (existing == null) {
            throw new Exception($"Todo with id {id} does not exist!");
        }

        context.Todos.Remove(existing);
        context.SaveChanges();
        return Task.CompletedTask;
    }
}