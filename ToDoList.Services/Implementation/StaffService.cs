using AutoMapper;
using ToDoList.Data.Extensions;
using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Dtos.Response;
using ToDoList.Models.Entities;
using ToDoList.Models.Enums;
using ToDoList.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data.Interfaces;
using ToDoList.Models.Entities;

namespace ToDoList.Services.Implementation;

public class StaffService : IStaffService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IServiceFactory _serviceFactory;
    private readonly IMapper _mapper;
    private readonly IRepository<UserProfile> _staffProfileRepo;
    private readonly IUnitOfWork _unitOfWork;

    public StaffService(IServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
        _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
        _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
        _staffProfileRepo = _unitOfWork.GetRepository<UserProfile>();
        _mapper = _serviceFactory.GetService<IMapper>();
    }

    public async Task<string> CreateStaff(CreateStaffRequest request)
    {
        if (request == null)
            throw new InvalidOperationException("Invalid data sent");

        ApplicationUser staffExist = await _userManager.FindByNameAsync(request.Email.Trim().ToLower());
        if (staffExist != null)
            return $"A user already exists with username {request.Email}";

        UserRegistrationRequest userRegistration = new()
        {
            Email = request.Email,
            UserName = request.Username,
            Role = request.Role,
            Firstname = request.FirstName,
            LastName = request.LastName,
            MobileNumber = request.PhoneNumber,
            UserTypeId = UserType.Staff
        };

        string userId = await _serviceFactory.GetService<IAuthenticationService>().CreateUser(userRegistration);
        UserProfile staff = _mapper.Map<UserProfile>(request);
        staff.UserId = userId;
        await _staffProfileRepo.AddAsync(staff);

        //Log.ForContext(new PropertyBagEnricher()
        //        .Add("NewUser", request, destructureObject: true))
        //    .Information("Creation Successful");

        return $"A link to verify your account has been sent to the email {request.Email} you provided";
    }

    public async Task<PagedResponse<StaffProfileResponse>> GetAllStaff(StaffProfileRequest request)
    {
        IQueryable<ApplicationUserRole> usersQueryable = string.IsNullOrWhiteSpace(request.RoleName)

            ? _userManager.Users.Include(x => x.UserRoles)
                .ThenInclude(x => x.User).Include(x => x.UserRoles).ThenInclude(x => x.Role).SelectMany(x =>
                    x.UserRoles).AsNoTracking()

            : _userManager.Users.Include(x => x.UserRoles)
            .ThenInclude(x => x.User).Include(x => x.UserRoles).ThenInclude(x => x.Role).SelectMany(x =>
                x.UserRoles.Where(r => r.Role.Name.ToLower() == request.RoleName.ToLower())).AsNoTracking();

        IQueryable<UserProfile> staffQueryable = _staffProfileRepo.GetQueryable().AsNoTracking();

        //IQueryable<LecturersProfile> lecturersQueryable = _lecturerRepo.GetQueryable().Include(x => x.Department).Include(x => x.StudentType).AsNoTracking();

        //IQueryable<StudentPersonalData> studentsQueryable = _studentRepo.GetQueryable()
        //    .Include(x => x.StudentProgrammeDetail).ThenInclude(x => x.Department)
        //    .Include(x => x.StudentProgrammeDetail).ThenInclude(x => x.DepartmentOption).Include(x => x.StudentProgrammeDetail).ThenInclude(x => x.StudentType).AsNoTracking();

        IQueryable<StaffProfileResponse> staffProfileResponseQueryable = usersQueryable
            .Join(staffQueryable, aur => aur.UserId, sbp => sbp.UserId,
                (aur, sbp) => new { AppUserRole = aur, StaffProfile = sbp })
            .Select(s => new StaffProfileResponse
            {
                UserId = s.AppUserRole.UserId,
                StaffId = s.StaffProfile.Id,
                LastName = s.StaffProfile.LastName,
                FirstName = s.StaffProfile.FirstName,
                MiddleName = s.StaffProfile.MiddleName,
                UserName = s.AppUserRole.User.UserName,
                DepartmentId = s.StaffProfile.DepartmentId,
              
                GenderId = s.StaffProfile.GenderId ?? 0,
                Gender = s.StaffProfile.GenderId == Gender.Male ? "Male" :
                    s.StaffProfile.GenderId == Gender.Female ? "Female" : "",
                Role = s.AppUserRole.Role.Name,
                Status = s.AppUserRole.User.Active == false || s.AppUserRole.User.PasswordHash == null ? "Deactivated" : "Activated",
                StudentTypeId = s.StaffProfile.StudentTypeId ?? 0,
                //StudentType = s.StaffProfile.StudentType.Name,
                Email = s.AppUserRole.User.Email,
                PhoneNumber = s.AppUserRole.User.PhoneNumber,
                //AllowImpersonation = s.AppUserRole.Role.Name != UserRoles.SuperAdmin

            });

        //IQueryable<StaffProfileResponse> lecturerProfileResponseQueryable = usersQueryable
        //    .Join(lecturersQueryable, aur => aur.UserId, lp => lp.UserId,
        //        (aur, lp) => new { AppUserRole = aur, LecturersProfile = lp })
        //    .Select(s => new StaffProfileResponse
        //    {
        //        UserId = s.AppUserRole.UserId,
        //        StaffId = s.LecturersProfile.Id,
        //        LastName = s.LecturersProfile.Lastname,
        //        FirstName = s.LecturersProfile.Firstname,
        //        MiddleName = s.LecturersProfile.Middlename,
        //        UserName = s.AppUserRole.User.UserName,
        //        DepartmentId = s.LecturersProfile.DepartmentId,
        //        Department = s.LecturersProfile.Department.Name,
        //        GenderId = s.LecturersProfile.GenderId ?? 0,
        //        Gender = s.LecturersProfile.GenderId == Gender.Male ? "Male" :
        //            s.LecturersProfile.GenderId == Gender.Female ? "Female" : "",
        //        Role = s.AppUserRole.Role.Name,
        //        Status = s.AppUserRole.User.Active == false || s.AppUserRole.User.PasswordHash == null ? "Deactivated" : "Activated",
        //        StudentTypeId = s.LecturersProfile.StudentTypeId,
        //        StudentType = s.LecturersProfile.StudentType.Name,
        //        Email = s.AppUserRole.User.Email,
        //        PhoneNumber = s.AppUserRole.User.PhoneNumber,
        //        AllowImpersonation = s.AppUserRole.Role.Name != UserRoles.SuperAdmin
        //    });

        //IQueryable<StaffProfileResponse> studentProfileResponseQueryable = usersQueryable
        //    .Join(studentsQueryable, aur => aur.UserId, sp => sp.UserId,
        //        (aur, sp) => new { AppUserRole = aur, StudentProfile = sp }).Select(s =>
        //        new StaffProfileResponse
        //        {
        //            UserId = s.AppUserRole.UserId,
        //            StaffId = s.StudentProfile.Id,
        //            LastName = s.StudentProfile.Lastname,
        //            FirstName = s.StudentProfile.Firstname,
        //            MiddleName = s.StudentProfile.Middlename,
        //            UserName = s.AppUserRole.User.UserName,
        //            StudentTypeId = s.StudentProfile.StudentProgrammeDetail.StudentTypeId,
        //            StudentType = s.StudentProfile.StudentProgrammeDetail.StudentType.Name,
        //            Email = s.AppUserRole.User.Email,
        //            PhoneNumber = s.AppUserRole.User.PhoneNumber,
        //            GenderId = s.StudentProfile.GenderId,
        //            Gender = s.StudentProfile.GenderId == Gender.Male ? "Male" :
        //                s.StudentProfile.GenderId == Gender.Female ? "Female" : "",
        //            DepartmentId = s.StudentProfile.StudentProgrammeDetail.DepartmentId,
        //            Department =
        //                s.StudentProfile.StudentProgrammeDetail.Department.Name + (s.StudentProfile.StudentProgrammeDetail.DepartmentOption == null
        //                    ? string.Empty
        //                    : " ( " + s.StudentProfile.StudentProgrammeDetail.DepartmentOption.Name + " )"),

        //            Role = s.AppUserRole.Role.Name,
        //            Status = s.AppUserRole.User.Active == false || s.AppUserRole.User.PasswordHash == null ? "Deactivated" : "Activated",
        //            AllowImpersonation = s.AppUserRole.Role.Name != UserRoles.SuperAdmin
        //        });


        PagedList<StaffProfileResponse> pagedList = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? await staffProfileResponseQueryable
            //.Concat(lecturerProfileResponseQueryable)
            //.Concat(studentProfileResponseQueryable)
            .OrderBy(s => s.LastName)
                .GetPagedItems(request)
            : await staffProfileResponseQueryable
            //.Concat(lecturerProfileResponseQueryable)
            //.Concat(studentProfileResponseQueryable)
            .OrderBy(s => s.LastName)
            .GetPagedItems(request, searchExpression: x => x.UserName.ToLower().Contains(request.SearchTerm.Trim().ToLower()));

        return _mapper.Map<PagedResponse<StaffProfileResponse>>(pagedList);

    }

    public async Task<StaffProfileResponse> GetSingleStaff(string id)
    {

        ApplicationUser user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            throw new InvalidOperationException($"The user with {nameof(id)}: {id} doesn't exist in the database.");
        }

        switch (user.UserTypeId)
        {
            case UserType.Staff:

                UserProfile staff = await _staffProfileRepo.GetSingleByAsync(s => s.UserId == id, include:
                    x => x.Include(u => u.User));

                if (staff != null)
                {

                    StaffProfileResponse staffResponse = _mapper.Map<StaffProfileResponse>(staff);

                    ApplicationUser staffUser = await _userManager.FindByIdAsync(staffResponse.UserId);

                    if (staffUser == null) return staffResponse;

                    IEnumerable<string> userRole = await _userManager.GetRolesAsync(staffUser);
                    staffResponse.Role = userRole.FirstOrDefault();

                    return staffResponse;
                }
                break;
            

            default:
                return null;
        }
        return null;

    }

    public async Task UpdateStaffProfile(string userId, UpdateStaffProfileRequest request)
    {

        ApplicationUser user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException($"The user with {nameof(userId)}: {userId} doesn't exist in the database.");
        }


        user.LastName = request.LastName;
        user.FirstName = request.FirstName;
        user.MiddleName = request.MiddleName;
        user.PhoneNumber = request.PhoneNumber;
        await _userManager.UpdateAsync(user);

        IEnumerable<string> userRoles = await _userManager.GetRolesAsync(user);
        if (!userRoles.Any(x => x.Contains(request.Role.Trim())))
        {
            string roleToRemove = userRoles.FirstOrDefault();
            await _userManager.RemoveFromRoleAsync(user, roleToRemove);
            await _userManager.AddToRoleAsync(user, request.Role.Trim());
        }

        switch (user.UserTypeId)
        {
            case UserType.Staff:

                UserProfile staff = await _staffProfileRepo.GetSingleByAsync(s => s.UserId == userId);

                if (staff == null)
                    throw new InvalidOperationException($"The user with {nameof(userId)}: {userId} doesn't exist in the database.");

                UserProfile staffUpdate = _mapper.Map(request, staff);

                await _staffProfileRepo.UpdateAsync(staffUpdate);

                //Log.ForContext(new PropertyBagEnricher()
                //        .Add("UpdateUser", new { staffId = staffUpdate.Id, request }, destructureObject: true))
                //    .Information("Update Successful");

                break;

            //case UserType.Lecturer:
            //    LecturersProfile Lecturer = await _lecturerRepo.GetSingleByAsync(l => l.UserId == userId);
            //    if (Lecturer == null)
            //        throw new InvalidOperationException("Lecturer not found");

            //    LecturersProfile lecturerToUpdate = _mapper.Map(request, Lecturer);
            //    await _lecturerRepo.UpdateAsync(lecturerToUpdate);

            //    Log.ForContext(new PropertyBagEnricher()
            //            .Add("UpdateUser", new { lecturerId = Lecturer.Id, request }, destructureObject: true))
            //        .Information("Update Successful");

            //    break;
            //case UserType.Student:

            //    await _serviceFactory.GetService<IStudentService>().AdminUpdateStudent(userId, request);

            //    Log.ForContext(new PropertyBagEnricher()
            //            .Add("UpdateUser", new { studentRefId = userId, request }, destructureObject: true))
            //        .Information("Update Successful");
            //    break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
