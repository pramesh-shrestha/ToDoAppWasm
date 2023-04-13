using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;

namespace Application.Logic; 

public class TodoLogic : ITodoLogic {

    private readonly ITodoDao todoDao;
    private readonly IUserDAO userDao;

    public TodoLogic(ITodoDao todoDao, IUserDAO userDao) {
        this.todoDao = todoDao;
        this.userDao = userDao;
    }

    public async Task<Todo> CreateAsync(TodoCreationDTO dto) {
        Task<User?> task = userDao.GetByIdAsync(dto.OwnerId); // Start the asynchronous operation
        User? user = await task; // Wait for the operation to complete and get the user object
        
        //What we actually doing is
        // User? user = await userDao.GetByIdAsync(dto.OwnerId);

        if (user == null) {
            throw new Exception($"User with id {dto.OwnerId} was not found.");
        }
        
        ValidateTodo(dto);
        Todo todo = new Todo(user, dto.Title);
        Todo created = await todoDao.CreateAsync(todo);
        return created;
    }

    public Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters) {
        return todoDao.GetAsync(searchParameters);
    }

    public async Task UpdateAsync(TodoUpdateDto dto) {
        Todo? existing = await todoDao.GetByIdAsync(dto.Id);
        if (existing == null) {
            throw new Exception($"Todo with ID {dto.Id} not found!");
        }

        User? user = null;
        if (dto.OwnerId != null) {
            user = await userDao.GetByIdAsync((int)dto.OwnerId); //we cast to int because nullable int and int are different
            if (user == null) {
                throw new Exception($"User with id {dto.OwnerId} was not found.");
            }
        }

        if (dto.IsCompleted != null && existing.IsCompleted && !(bool)dto.IsCompleted) {
            throw new Exception("Cannot un-complete a completed Todo");
        }

        // It uses the null-coalescing operator (??) to assign a value to userToUser based on the values of user and existing.Owner.
        // If user is not null, userToUser is assigned the value of user. If user is null, userToUser is assigned the value of existing.Owner.
        User userToUser = user ?? existing.Owner;
        string titleToUse = dto.Title ?? existing.Title;
        bool completedToUse = dto.IsCompleted ?? existing.IsCompleted;

        Todo updated = new(userToUser, titleToUse) {
            IsCompleted = completedToUse,
            Id = existing.Id
        };
        
        ValidateTodo(updated);
        await todoDao.UpdateAsync(updated);
    }
    

    //get specific to do by id
    public async Task<TodoGetByIdDto> GetTodoById(int id) {
        Todo todo = await todoDao.GetByIdAsync(id);
        if (todo == null) {
            throw new Exception($"There is no todo with {id}");
        }

        TodoGetByIdDto todoById = new TodoGetByIdDto(todo.Owner.UserName, todo.Id, todo.Title,todo.IsCompleted);
        return todoById;
    }

    public async Task DeleteAsyncById(int id) {
        await todoDao.DeleteAsyncById(id);
    }


    private void ValidateTodo(Todo dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new Exception("Title cannot be empty.");
    }

    private void ValidateTodo(TodoCreationDTO dto) {
        if(string.IsNullOrEmpty(dto.Title)) throw new Exception("Title cannot be empty.");
    }
}   