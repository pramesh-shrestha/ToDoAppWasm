using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Domain.DataTransferObjects;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations; 

public class TodoHttpClient : ITodoService {
    private readonly HttpClient client;

    public TodoHttpClient(HttpClient client) {
        this.client = client;
    }

    public async Task CreateAsync(TodoCreationDTO dto) {
        HttpResponseMessage response = await client.PostAsJsonAsync("/todos", dto);
        if (!response.IsSuccessStatusCode) {
            string result = await response.Content.ReadAsStringAsync();
            throw new Exception(result);
        }
    }

    public async Task<ICollection<Todo>> GetAsync(string? userName, int? userId , bool? completedStatus, string? titleContains) {

        string query = ConstructQuery(userName, userId, completedStatus, titleContains);
        
        HttpResponseMessage response = await client.GetAsync($"/todos" + query);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) {
            throw new Exception(result);
        }
        ICollection<Todo> todos = JsonSerializer.Deserialize<ICollection<Todo>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return todos;
    }

    //The query string must always start with a "?", and each query is separated with "&".
    // Results could for example be:
    // "?username=Troels&titlecontains=hej"
    //"?userid=3"
    //"?completedstatus=false&titlecontains=hej"
    private static string ConstructQuery(string? userName, int? userId, bool? completedStatus, string? titleContains) {
        string query = "";
        if (!string.IsNullOrEmpty(userName)) {
            query += $"?username={userName}";
            Console.WriteLine("username");
        }

        if (userId != null) {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"userid = {userId}";
            Console.WriteLine("userId");
        }

        if (completedStatus != null) {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"completedstatus={completedStatus}";
            Console.WriteLine("completedStatus");
        }

        if (!string.IsNullOrEmpty(titleContains)) {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"titlecontains={titleContains}";
            Console.WriteLine("title contains");
        }

        return query;
    }
    
    //Update
    public async  Task UpdateAsync(TodoUpdateDto dto) {
        string dtoAsJson = JsonSerializer.Serialize(dto);
        //StringContent to hold the data in the body of the request message. We provide as arguments first the "dto as JSON",
        //then the encoding, and finally the format of the string. We are sending JSON, so we pass "application/json".
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PatchAsync("/todos", body);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    //get by id
    public async Task<TodoGetByIdDto> GetByIdAsync(int id) {
        HttpResponseMessage response = await client.GetAsync($"/todos/{id}");
        string result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            throw new Exception(result);
        }

        TodoGetByIdDto todo = JsonSerializer.Deserialize<TodoGetByIdDto>(result, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!; //This part of the code uses the null-forgiving operator (!) to indicate that the result of the deserialization operation is expected to be non-null.
             //If the deserialization operation returns null, a NullReferenceException will be thrown.
        return todo;
        
    }

    public async Task DeleteAsync(int id) {
        HttpResponseMessage response = await client.DeleteAsync($"todos/{id}");
        
        if (!response.IsSuccessStatusCode) {
            string result = await response.Content.ReadAsStringAsync();
            throw new Exception(result);
        }
    }
  
}