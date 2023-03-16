using ToDoList.Models.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.Dtos.Request;

public class StaffProfileRequest : RequestParameters
{
    public StaffProfileRequest() => OrderBy = "Surname";

    [Required]
    public string RoleName { get; set; }
}

public class CreateStaffRequest
{
    [Required(ErrorMessage = "LastName cannot be empty"), RegularExpression(@"^[\w ]*[a-zA-Z]+(([', -][a-zA-Z])?[a-zA-Z]*)\s*$",
         ErrorMessage = "Invalid Firstname !"), MaxLength(20), MinLength(2)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "First Name cannot be empty"), RegularExpression(@"^[\w ]*[a-zA-Z]+(([', -][a-zA-Z])?[a-zA-Z]*)\s*$",
         ErrorMessage = "Invalid Lastname !"), MaxLength(20), MinLength(2)]
    public string FirstName { get; set; }

    [RegularExpression(@"^[\w ]*[a-zA-Z]+(([', -][a-zA-Z])?[a-zA-Z]*)\s*$", ErrorMessage = "Invalid Middle name !")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required"), EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public long DepartmentId { get; set; }

    [Required]
    public string Role { get; set; }
}

public class UpdateStaffProfileRequest
{
    [Required(ErrorMessage = "LastName cannot be empty"), RegularExpression(@"^[\w ]*[a-zA-Z]+(([', -][a-zA-Z])?[a-zA-Z]*)\s*$",
         ErrorMessage = "Invalid Firstname !"), MaxLength(20), MinLength(2)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "First Name cannot be empty"), RegularExpression(@"^[\w ]*[a-zA-Z]+(([', -][a-zA-Z])?[a-zA-Z]*)\s*$",
         ErrorMessage = "Invalid Lastname !"), MaxLength(20), MinLength(2)]
    public string FirstName { get; set; }

    [RegularExpression(@"^[\w ]*[a-zA-Z]+(([', -][a-zA-Z])?[a-zA-Z]*)\s*$", ErrorMessage = "Invalid Middle name !")]
    public string? MiddleName { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public long DepartmentId { get; set; }

    [Required]
    public int StudentTypeId { get; set; }

    [Required]
    public string Role { get; set; }
}