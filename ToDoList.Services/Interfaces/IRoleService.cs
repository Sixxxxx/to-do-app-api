using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Dtos.Response;

namespace ToDoList.Services.Interfaces;

public interface IRoleService
{
    Task AddUserToRole(AddUserToRoleRequest request);
    Task CreateRole(RoleDto request);
    Task DeleteRole(RoleDto request);
    Task EditRole(string id, RoleDto request);
    Task RemoveUserFromRole(AddUserToRoleRequest request);
    Task<IEnumerable<string>> GetUserRoles(string userName);
    Task<PagedResponse<RoleResponse>> GetAllRoles(RoleRequestDto request);
    //Task<IEnumerable<MenuClaimsResponse>> GetRoleClaims(string roleName);
    Task UpdateRoleClaims(UpdateRoleClaimsDto request);
}