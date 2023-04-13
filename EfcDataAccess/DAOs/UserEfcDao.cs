using Application.DAOInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs; 

public class UserEfcDao : IUserDAO {

    private readonly ToDoContext context;

    public UserEfcDao(ToDoContext context) {
        this.context = context;
    }

    //The database sets the Id of the User, which is why it may be relevant to return the created User, now with the correct Id.
    public async Task<User> CreateAsync(User user) {
        EntityEntry<User> newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return newUser.Entity;
    }

    public async Task<User?> GetByUsernameAsync(string userName) {
        User? existingUser =
            await context.Users.FirstOrDefaultAsync(user => user.UserName.ToLower().Equals(userName.ToLower()));

        return existingUser;
    }

    public async Task<IEnumerable<User>> GetAsync(SearchUserParameterDTO searchParameters) {
        IQueryable<User> usersQuery = context.Users.AsQueryable();
        if (searchParameters.UsernameContains != null) {
            usersQuery = usersQuery
                .AsNoTracking()
                .Where((user =>
                user.UserName.ToLower().Contains(searchParameters.UsernameContains.ToLower())));
        }
        
        //Only when we use the result, i.e. by converting to list, do we actually execute the query against the database.
        //This is an important point. If you initially convert to list or similar, you'll load the entire table. This is not efficient
        IEnumerable<User> result = await usersQuery.ToListAsync();
        return result;
    }

    public async Task<User?> GetByIdAsync(int dtoOwnerId) {
        //We can use FindAsync instead of FirstOrDefaultAsync if the argument is a primary key
        User? user = await context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == dtoOwnerId);
        return user;
    }
}