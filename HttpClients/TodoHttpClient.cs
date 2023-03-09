using System.Text.Json;
using Domain.Models;
using System.Net.Http;
using System.Text;

namespace HttpClients; 

public class TodoHttpClient {
    public async Task<ICollection<Todo>> GetAsync()
    {
        using HttpClient client = new();

        HttpResponseMessage response = await client.GetAsync("https://localhost:7038/todos");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }
        ICollection<Todo> todos = JsonSerializer.Deserialize<ICollection<Todo>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return todos;
    }
    
    public async Task<Todo> AddAsync(Todo todo)
    {
        using HttpClient client = new();
        string todoAsJson = JsonSerializer.Serialize(todo);
        StringContent content = new(todoAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync("https://localhost:7204/todos", content);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }
        Todo returned = JsonSerializer.Deserialize<Todo>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return returned;
    }
}   