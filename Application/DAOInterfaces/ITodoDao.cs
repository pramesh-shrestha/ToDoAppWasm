using Domain.DataTransferObjects;
using Domain.Models;

namespace Application.DAOInterfaces; 

public interface ITodoDao {
    Task<Todo> CreateAsync(Todo todo);
    Task<IEnumerable<Todo>> GetAsync(SearchTodoParametersDto searchParameters);
    //The DAO interface needs a Todo object, and there is no need to return anything. This means the Logic implementation will convert from TodoUpdateDto to Todo.
    Task UpdateAsync(Todo todo);
    //To retrieve a single Todo given and Id
    Task<Todo> GetByIdAsync(int id);
    Task DeleteAsync(TodoDeleteDto dto);
  
}