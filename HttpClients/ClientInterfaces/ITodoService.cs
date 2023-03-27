using Domain.DataTransferObjects;
using Domain.Models;

namespace HttpClients.ClientInterfaces; 

public interface ITodoService {
    Task CreateAsync(TodoCreationDTO dto); //If is only Task with async it means it void return type
    Task<ICollection<Todo>> GetAsync(
        string? userName = null, 
        int? userId = null, 
        bool? completedStatus = null, 
        string? titleContains = null
    );

    Task UpdateAsync(TodoUpdateDto dto);

    Task<TodoGetByIdDto> GetByIdAsync(int id);

    Task DeleteAsync(int id);
}

