using ToDoList.Models.Enums;

namespace ToDoList.Models.Dtos.Response;

public class StaffProfileResponse
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public long StaffId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public Gender GenderId { get; set; }
    public string Gender { get; set; }
    public long DepartmentId { get; set; }
    public string Department { get; set; }
    public string Role { get; set; }
    public string Status { get; set; }
    public int? StudentTypeId { get; set; }
    public string StudentType { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName => $"{LastName} {FirstName} {MiddleName}";
    public bool AllowImpersonation { get; set; }
}