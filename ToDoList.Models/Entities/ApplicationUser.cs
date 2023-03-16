using ToDoList.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? RecoveryMail { get; set; }
        public DateTime? Birthday { get; set; }
        public UserType UserTypeId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
