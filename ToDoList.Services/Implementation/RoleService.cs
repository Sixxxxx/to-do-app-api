using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data.Extensions;
using ToDoList.Data.Interfaces;
using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Dtos.Response;
using ToDoList.Models.Entities;
using ToDoList.Services.Interfaces;
using System.Security.Claims;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementation;

public class RoleService : IRoleService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IServiceFactory _serviceFactory;
    private readonly IMapper _mapper;
    private readonly IRepository<ApplicationRole> _roleRepo;
    private readonly IRepository<ApplicationRoleClaim> _roleClaimRepo;
    
    private readonly IUnitOfWork _unitOfWork;


    public RoleService(IServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
        _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
        _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
        _roleManager = _serviceFactory.GetService<RoleManager<ApplicationRole>>();
        _roleRepo = _unitOfWork.GetRepository<ApplicationRole>();
        _roleClaimRepo = _unitOfWork.GetRepository<ApplicationRoleClaim>();
        _mapper = _serviceFactory.GetService<IMapper>();
    }

    public async Task AddUserToRole(AddUserToRoleRequest request)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(request.UserName.Trim().ToLower());

        if (user == null)
            throw new InvalidOperationException($"User '{request.UserName}' does not Exist!");

        await _userManager.AddToRoleAsync(user, request.Role.ToLower().Trim());

       
    }

    public async Task CreateRole(RoleDto request)
    {
        ApplicationRole role = await _roleManager.FindByNameAsync(request.Name.Trim().ToLower());

        if (role != null)
            throw new InvalidOperationException($"Role with name {request.Name} already exist");

        ApplicationRole roleToCreate = _mapper.Map<ApplicationRole>(request);

        await _roleManager.CreateAsync(roleToCreate);


      
    }

    public async Task DeleteRole(RoleDto request)
    {
        ApplicationRole role = await _roleManager.FindByNameAsync(request.Name.Trim().ToLower());

        if (role is null)
            throw new InvalidOperationException($"Role {request.Name} does not Exist");

        await _roleManager.DeleteAsync(role);


     
    }

    public async Task EditRole(string id, RoleDto request)
    {
        ApplicationRole role = await _roleManager.FindByIdAsync(id);
        if (role == null)
            throw new InvalidOperationException($"Role with {id} not found");

        ApplicationRole roleUpdate = _mapper.Map(request, role);

        await _roleManager.UpdateAsync(roleUpdate);

       
    }

    public async Task RemoveUserFromRole(AddUserToRoleRequest request)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(request.UserName.Trim().ToLower());

        if (user == null)
            throw new InvalidOperationException($"User {request.UserName} does not exist");

        bool userIsInRole = await _roleRepo.GetQueryable().Include(x => x.UserRoles).ThenInclude(x => x.Role)
            .AnyAsync(r => r.UserRoles.Any(ur => ur.Role.Active));


        if (!userIsInRole)
            throw new InvalidOperationException($"User not in {request.Role} Role");

        await _userManager.RemoveFromRoleAsync(user, request.Role);

       
    }

    public async Task<IEnumerable<string>> GetUserRoles(string userName)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(userName.Trim().ToLower());
        if (user == null)
            throw new InvalidOperationException($"User with username {userName} not found");

        List<string> userRoles = (List<string>)await _userManager.GetRolesAsync(user);
        if (!userRoles.Any())
            throw new InvalidOperationException($"User {userName} has no role");

        return userRoles;
    }

    public async Task<PagedResponse<RoleResponse>> GetAllRoles(RoleRequestDto request)
    {
        IQueryable<ApplicationRole> roleQueryable = _roleRepo.GetQueryable().Include(x => x.UserRoles)
            .ThenInclude(x => x.Role).Include(x => x.RoleClaims);

        roleQueryable = !request.Active ? roleQueryable : roleQueryable.Where(r => r.Active);

        IQueryable<RoleResponse> roleResponseQueryable = roleQueryable.Select(s => new RoleResponse
        {
            Name = s.Name,
            ClaimCount = s.RoleClaims.Count,
            Active = s.Active
        });

        PagedList<RoleResponse> pagedList = !string.IsNullOrWhiteSpace(request.SearchTerm)
            ? await roleResponseQueryable.GetPagedItems(request,
                searchExpression: r => r.Name.ToLower().Contains(request.SearchTerm.ToLower()) && r.Active)
            : await roleResponseQueryable.GetPagedItems(request);

        return _mapper.Map<PagedResponse<RoleResponse>>(pagedList);

    }

    //public async Task<IEnumerable<MenuClaimsResponse>> GetRoleClaims(string roleName)
    //{
    //    IEnumerable<string> claimsInRole =
    //        (await _roleRepo.GetQueryable().Include(x => x.UserRoles)
    //            .Include(x => x.RoleClaims).Where(r => r.Name.ToLower() == roleName.ToLower() && r.Active)
    //            .FirstOrDefaultAsync())?.RoleClaims.Select(s => s.ClaimValue) ?? new List<string>();


    //    IEnumerable<MenuClaimsResponse> menuClaims =
    //        (await _menuRepo.GetQueryable(m => m.Claims != null && m.Active).ToListAsync())
    //        .Where(m => claimsInRole != null && m.Claims.Intersect(claimsInRole).Any()).GroupBy(x => x.Name).Select(s =>
    //            new MenuClaimsResponse
    //            {
    //                Menu = s.Key,
    //                Claims = s.SelectMany(_ => _.Claims).ToList()
    //            });

    //    return menuClaims;
    //}

    public async Task UpdateRoleClaims(UpdateRoleClaimsDto request)
    {
        ApplicationRole? role = await _roleRepo.GetQueryable().Include(x => x.RoleClaims).FirstOrDefaultAsync(r => r.Name.ToLower() == request.Role.ToLower());

        if (role == null)
            throw new InvalidOperationException("Role does not exist");

        role.RoleClaims.Clear();

        IList<ApplicationRoleClaim> roleClaims = new List<ApplicationRoleClaim>(request.Claims.Count);

        foreach (string requestClaim in request.Claims)
        {
            roleClaims.Add(new ApplicationRoleClaim { ClaimType = ClaimTypes.Name, ClaimValue = requestClaim, RoleId = role.Id });

        }

        await _roleClaimRepo.AddRangeAsync(roleClaims);

        await _unitOfWork.SaveChangesAsync();

     
    }
}