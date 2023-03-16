using Microsoft.AspNetCore.Identity;
namespace ToDoList.Models.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; } = true;

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}