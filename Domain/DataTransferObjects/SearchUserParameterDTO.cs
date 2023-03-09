namespace Domain.DataTransferObjects; 

public class SearchUserParameterDTO {
    public string? UsernameContains { get; }

    //The property is marked with "?", i.e. string? to indicate this search parameter can be null, i.e. it should be ignored when searching users.
    public SearchUserParameterDTO(string? usernameContains) {
        UsernameContains = usernameContains;
    }

}