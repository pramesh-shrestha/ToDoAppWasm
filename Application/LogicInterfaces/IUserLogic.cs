using Domain.DataTransferObjects;
using Domain.Models;

namespace Application.LogicInterfaces; 

//This is the access point to the domain logic

public interface IUserLogic {
    //The return type is Task<User> because we may want to do some work asynchronously.
    //There is nothing yet, but when the database is attached and we use EFC, things will have to be asynchronous.
    Task<User> CreateAsync(UserCreationDTO userToCreate);
    
    //We return an IEnumerable. IEnumerable is just a kind of simple, non-modifiable collection.
    //We could use ICollection, IList or something else, but there is a tendency to return IEnumerable for these kinds of things.
    Task<IEnumerable<User>> GetAsync(SearchUserParameterDTO searchParameters);
}