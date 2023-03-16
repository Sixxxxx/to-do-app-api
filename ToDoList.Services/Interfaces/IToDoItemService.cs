using Microsoft.AspNetCore.JsonPatch;
using ToDoList.Models.Dtos.Requests;
using ToDoList.Models.Entities;

namespace ToDoList.Services.Interfaces
{
    public interface IToDoItemService
    {
        Task<IEnumerable<ToDoItem>> GetAllToDoItems();
        Task<ToDoItem> GetToDoItem(int Id);
        Task CreateToDoItem(CreateToDoItemRequest request);
        Task UpdateToDoItem(int Id,CreateToDoItemRequest request);
        Task DeleteToDoItem(int Id);
        Task ToggleToDoItem(int Id);
        Task PatchToDoItem(int Id, JsonPatchDocument<CreateToDoItemRequest> request);
    }
}
