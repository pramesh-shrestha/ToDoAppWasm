namespace Domain.DataTransferObjects; 

public class TodoCreationDTO {
    public int OwnerId { get; }
    public string Title { get; }

    public TodoCreationDTO(int ownerId, string title) {
        OwnerId = ownerId;
        Title = title;
    }
}