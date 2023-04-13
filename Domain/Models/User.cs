using System.Text.Json.Serialization;

namespace Domain.Models; 

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    
    //We have added two-way navigation properties to the domain classes, i.e. Todo associates User, and User associates Todo.
    //The Web API will return JSON. We cannot serialize objects to JSON if there are circular dependencies, which is what we have.
    //It is a Shadow Property, i.e. a property that does not really exist, but EFC sneakily adds it.
    [JsonIgnore]
    public ICollection<Todo> Todos { get; set; }
}