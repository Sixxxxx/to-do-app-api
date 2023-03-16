using ToDoList.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ToDoList.Data.Context
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, IdentityUserLogin<string>, ApplicationRoleClaim,
        IdentityUserToken<string>>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
           : base(options)
        {
        }

        public virtual DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
