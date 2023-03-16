namespace ToDoList.Models.Dtos.Response;

public class MenuClaimsResponse
{
    public string Menu { get; set; }
    public IEnumerable<string> Claims { get; set; }
}