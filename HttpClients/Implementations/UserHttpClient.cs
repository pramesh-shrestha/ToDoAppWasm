using System.Data.SqlTypes;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DataTransferObjects;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations; 

public class UserHttpClient : IUserService {
    private readonly HttpClient client;

    public UserHttpClient(HttpClient client) {
        this.client = client;
    }

    public async Task<User> Create(UserCreationDTO dto) {
        //PostAsJsonAsync - The "PostAsJsonAsync" method is an extension method provided by the "HttpClient" class in .NET
        //that serializes an object to JSON format and sends it as the request body to the specified URL.
        //It takes two parameters: the URL of the API endpoint, and the data to be sent in the request body.
        
        //This line of code sends an HTTP POST request to a web API endpoint located at "/User" using the HTTP client represented by the variable "client".
        //The request body contains serialized JSON data derived from the object represented by the variable "dto".
        //The await keyword indicates that the method execution will wait until the operation is completed, and the result is available before moving on to the next line.
        //The result is the HttpContent.
        HttpResponseMessage response = await client.PostAsJsonAsync("/User", dto); 
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        // we know the result is a User as JSON, and it is deserialized and returned.
        //We supply the JsonSerializer with options to ignore casing, because the result from the Web API will be camelCase,
        //but our model classes use PascalCase for the properties.
        
        User user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!; //"!", i.e. the exclamation mark. This is because, the Deserialize method returns a nullable object, i.e. User?,
             //but we just above checked if the request went well, so at this point we know there is a User to be deserialized.
        return user;
    }

    //The method is async, because we make a call to the Web API, which may take time. The argument is again defaulted to null here,
    //like in the interface. And the return type is IEnumerable<Users>, i.e. the immutable collection returned from the Web API endpoint.
    //(We can actually deserialize the JSON into any kind of collection, it need not be IEnumerable).
    public async Task<IEnumerable<User>> GetUsers(string? usernameContains = null) {
       //First the sub-URI is defined to be "/users".   
        string uri = "/user";
        
        //Then, if the method-argument is not null, we suffix that to the URI as a query parameter.
        //The URI might then e.g. be: /users?username=roe, to fetch all users whose name contains "roe".
        if (!string.IsNullOrEmpty(usernameContains)) {
            uri += $"?username={usernameContains}";
        }

        //The HttpClient instance client is used to send an HTTP GET request to the API endpoint with the URI constructed in the previous steps,
        //and stored in response. It returns the list of users if usernameContains is not null.
        HttpResponseMessage response = await client.GetAsync(uri);
        
        //The response from the API is read as a string using ReadAsStringAsync() method
        // because the response content can be in various formats such as JSON, XML, HTML, plain text, etc.,
        // and the ReadAsStringAsync() method reads the content as a string, regardless of the content format.
        
        string result = await response.Content.ReadAsStringAsync(); //he content of the response can be accessed through the Content property of the HttpResponseMessage object
        
        //If the response is unsuccessful, an exception is thrown with the response content as the exception message.
        if (!response.IsSuccessStatusCode) {
            throw new Exception(result);
        }

        //JsonSerializer class is used to deserialize the JSON response into an IEnumerable<User> collection using the Deserialize method
        IEnumerable<User> users = JsonSerializer.Deserialize<IEnumerable<User>>(result, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
        //Finally, the method returns the deserialized IEnumerable<User> collection.
        return users;
    }
}