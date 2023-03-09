using Application.LogicInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers; 

[ApiController] //This attribute marks this class as a Web API controller, so that the Web API framework will know about our class.
[Route("[controller]")] //specifies the sub-URI to access this controller class. With that "route template", the URI will be localhost:port/user
                                //We can define our own path with fx [Route("api/users")], and then the URI would be localhost:port/api/users.
                                //It is up to you whether you just stick to the default name, or pick something else.

public class UserController : ControllerBase{ //The class extends ControllerBase to get access to various utility methods.
                                    
    private readonly IUserLogic userLogic;  

    public UserController(IUserLogic userLogic) { //Then a field variable, injected through the constructor, so we can get access to the application layer, i.e. the logic.
        this.userLogic = userLogic;
    }

    //Method for endpoint. It should take the relevant data, pass it on to the logic layer, and return the result back to the client.
    [HttpPost] //we mark the method as [HttpPost] to say that POST requests to /users should hit this endpoint.
    public async Task<ActionResult<User>> CreateAsync(UserCreationDTO dto) {
        try
        {
            User user = await userLogic.CreateAsync(dto);
            return Created($"/users/{user.Id}", user); //User is then returned, with the method Created(),
                                                       //which will create an ActionResult with status code 201, the new path to this specific User 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    //We mark the method with [HttpGet] so that GET requests to this controller ends here.
    [HttpGet]
    //The argument is marked as [FromQuery] to indicate that this argument should be extracted from the query parameters of the URI.
    //A URI could look like:
    // https://localhost:7093/Users?username=roe
    // Indicating that we wish to filter the result by the user names which contains the text "roe".
    // Or if we want all users, we would use the URI:
    // https://localhost:7093/Users
    // If we later added other search parameters, e.g. age, we could have a URI like:
    // https://localhost:7093/Users?username=roe&age=25
    // Which would result in all users where the user name contains "roe" and their age is 25
    public async Task<ActionResult<IEnumerable<User>>> GetAsync([FromQuery] string? username) {
        try {
            SearchUserParameterDTO parameters = new(username);
            IEnumerable<User> users = await userLogic.GetAsync(parameters);
            return Ok(users);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
        
    }
}