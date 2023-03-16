using ToDoList.Models.Entities;

namespace ToDoList.Models.Dtos.Response;

public class UserMailResponse
{
    public ApplicationUser User { get; set; }
    public string FirstName { get; set; }
}