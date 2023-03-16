using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Dtos.Response;

namespace ToDoList.Services.Interfaces;

public interface IStaffService
{
    Task<string> CreateStaff(CreateStaffRequest request);
    Task<PagedResponse<StaffProfileResponse>> GetAllStaff(StaffProfileRequest request);
    Task<StaffProfileResponse> GetSingleStaff(string id);
    Task UpdateStaffProfile(string userId, UpdateStaffProfileRequest request);
}