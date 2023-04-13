using Domain.DataTransferObjects;
using Domain.Models;

namespace Application.LogicInterfaces; 

public interface ITodoLogic {
    //The return type is Task<User> because we may want to do some work asynchronously.
    //There is nothing yet, but when the database is attached and we use EFC, things will have to be asynchronous.
    Task<Todo> CreateAsync(TodoCreationDTO dto);
    
    //We return an IEnumerable. IEnumerable is just a kind of simple, non-modifiable collection.
    //We could use ICollection, IList or something else, but there is a tendency to return IEnumerable for these kinds of things.
    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters);

    Task UpdateAsync(TodoUpdateDto dto);
    
    Task<TodoGetByIdDto> GetTodoById(int id);

    Task DeleteAsyncById(int id);
}