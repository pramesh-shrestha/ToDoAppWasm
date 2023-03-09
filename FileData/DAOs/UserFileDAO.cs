using Application.DAOInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;

namespace FileData.DAOs; 

public class UserFileDAO : IUserDAO{
    
    private readonly FileContext context;

    public UserFileDAO(FileContext context)
    {
        this.context = context;
    }
    public Task<User> CreateAsync(User user) {
        int userId = 1;
        
        //The Any() method is called on the Users property of the context object.
        //This method returns a Boolean value that indicates whether the Users table contains any rows.
        //If the Users table is not empty, the code proceeds to the next step.
        if (context.Users.Any()) {
            //The Max() method is called on the Users property of the context object, passing in a lambda expression as an argument.
            //The lambda expression u => u.Id specifies that the Max() method should return the maximum value of the Id field in the Users table.
            //The value returned by the Max() method is assigned to the userId variable.
            userId = context.Users.Max(u => u.Id);
            userId++;
        }
        user.Id = userId;
        context.Users.Add(user);
        context.SaveChanges();
        //Using Task.FromResult() is often used as a way to return a completed Task
        //in asynchronous methods that don't perform any asynchronous operations themselves.
        return Task.FromResult(user); // returns the newly created User object as a task result.
    }

    public Task<User?> GetByUsernameAsync(string userName) {
        User? user = null;
        if (context.Users.Any()) {
            foreach (var u in context.Users) {
                if (u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)) {
                    return Task.FromResult(u);
                }
            }
        }
        return Task.FromResult(user);
        
        //The FirstOrDefault() method will find the first object matching the criteria specified in the lambda expression.
        //If nothing is found, null is returned.
        User? existing = context.Users.FirstOrDefault(u =>
            u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
        );
        return Task.FromResult(existing);
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParameterDTO searchParameters) {
        //The first line of code takes the users from the context, and converts that ICollection to an IEnumerable

        //Alternative Troel's way
        IEnumerable<User> userss = context.Users.AsEnumerable();
        if (searchParameters.UsernameContains != null)
        {
            userss = context.Users.Where(u => u.UserName.Contains(searchParameters.UsernameContains, StringComparison.OrdinalIgnoreCase));
        }
        
        return Task.FromResult(userss);
        
        IEnumerable<User> users = context.Users.AsEnumerable();
        
        //new list
        if (searchParameters.UsernameContains != null) {
            foreach (User user in context.Users) {
                if (user.UserName.Contains(searchParameters.UsernameContains, StringComparison.OrdinalIgnoreCase)) {
                    //add usr to list
                    users = context.Users;
                }
            }
        }   
        return Task.FromResult(users);
    }

    public Task<User?> GetByIdAsync(int dtoOwnerId) {
        User? existing = context.Users.FirstOrDefault(u => 
            u.Id == dtoOwnerId);
        return Task.FromResult(existing); //It returns the Task<User> object that contains the user that matches above condition
    }
}
