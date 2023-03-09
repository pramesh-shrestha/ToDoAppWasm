using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;

namespace Application.Logic; 

//This class must implement the IUserLogic interface, and it will depend on some way of storing data, i.e. the IUserDao interface
public class UserLogic : IUserLogic {
    private readonly IUserDAO userDao;

    public UserLogic(IUserDAO userDao) { //The User DAO is received through constructor dependency injection.
        this.userDao = userDao;
    }


    public async Task<User> CreateAsync(UserCreationDTO dto) {
        User? existing = await userDao.GetByUsernameAsync(dto.UserName);
        if (existing != null) {
            throw new Exception("Username already take!");
        }

        ValidateData(dto);
        User toCreate = new() {
            UserName = dto.UserName
        };

        User created = await userDao.CreateAsync(toCreate);
        return created;
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParameterDTO searchParameters) {
        //Notice that userDao.GetAsync(searchParameters) returns a Task, but we don't need to await it,
        //because we do not need the result here. Instead, we actually just returns that task, to be awaited somewhere else.
        return userDao.GetAsync(searchParameters);
    }

    private static void ValidateData(UserCreationDTO userToCreate) {
        string userName = userToCreate.UserName;
        if (userName.Length < 3)
            throw new Exception("Username must be at least 3 characters!");

        if (userName.Length > 15)
            throw new Exception("Username must be less than 16 characters!");
    }
}