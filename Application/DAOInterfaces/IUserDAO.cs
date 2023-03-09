using Domain.DataTransferObjects;
using Domain.Models;

namespace Application.DAOInterfaces; 
//We know that we need to store the new User, which we have just created. Let's define the DAO interface for this.

//Here, we take a User object and return a User object. That means the responsibility of converting from UserCreationDto to User lies in the application layer.
public interface IUserDAO {
    Task<User> CreateAsync(User user);
    Task<User?> GetByUsernameAsync(string userName);
    Task<IEnumerable<User>> GetAsync(SearchUserParameterDTO searchParameters);
    Task<User?> GetByIdAsync(int dtoOwnerId);
    
}