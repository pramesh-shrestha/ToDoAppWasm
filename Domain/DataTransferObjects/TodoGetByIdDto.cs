using System.Globalization;

namespace Domain.DataTransferObjects; 

public class TodoGetByIdDto {
    public string Username { get;}
    public int Id { get; }
    public string Title { get; }
    public bool IsCompleted { get; }

    public TodoGetByIdDto(string username, int id, string title, bool isCompleted) {
        Username = username;
        Id = id;
        Title = title;
        IsCompleted = isCompleted;
    }
}