namespace Domain.DataTransferObjects; 

public class TodoDeleteDto {
    public int Id { get; }

    public TodoDeleteDto(int id) {
        Id = id;
    }
}