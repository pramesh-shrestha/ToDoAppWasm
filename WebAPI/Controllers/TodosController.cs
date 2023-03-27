using Application.Logic;
using Application.LogicInterfaces;
using Domain.DataTransferObjects;
using Domain.Models;
using FileData;
using FileData.DAOs;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers; 

[ApiController]//This attribute marks this class as a Web API controller, so that the Web API framework will know about our class.
[Route("[controller]")] //specifies the sub-URI to access this controller class. With that "route template", the URI will be localhost:port/todos
                                //We can define our own path with fx [Route("api/todos")], and then the URI would be localhost:port/api/users.
                                //It is up to you whether you just stick to the default name, or pick something else.

public class TodosController : ControllerBase { //The class extends ControllerBase to get access to various utility methods.
    private readonly ITodoLogic todoLogic;

    public TodosController(ITodoLogic todoLogic) {
        this.todoLogic = todoLogic;
    }

    //Endpoint
    [HttpPost] //we mark the method as [HttpPost] to say that POST requests to /users should hit this endpoint.
    public async Task<ActionResult<Todo>> CreateAsync([FromBody]TodoCreationDTO dto) {
        
        try {
            Todo todo = await todoLogic.CreateAsync(dto);
            return Created($"/todos/{todo.Id}", todo);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetAsync([FromQuery] string? userName,
        [FromQuery] int? userId, [FromQuery] bool? completedStatus, [FromQuery] string? titleContains) {
        try {
            Console.WriteLine("Get async");
            SearchTodoParametersDto parameters = new SearchTodoParametersDto(userName,userId,completedStatus,titleContains);
            IEnumerable<Todo> todos = await todoLogic.GetAsync(parameters);
            return Ok(todos);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //update
    [HttpPatch]
    public async Task<ActionResult> Update([FromBody]TodoUpdateDto dto) {

        try {
            await todoLogic.UpdateAsync(dto);
            return Ok();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //delete
    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] TodoDeleteDto dto) {
        try {
            await todoLogic.DeleteAsync(dto);
            return Ok();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //delete by id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id) {
        try {
            await todoLogic.DeleteAsyncById(id);
            return Ok();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]  //The part in the parenthesis is the sub-uri, indicating you here put an id of type int.
    public async Task<ActionResult<TodoGetByIdDto>> GetById([FromRoute] int id) { //FromRoute, meaning the id is found i URI
        try {
            return Ok( await todoLogic.GetTodoById(id));
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}