
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.Dtos.Request
{
    public class UpdateRoleClaimsDto
    {
        public string Role { get; set; }
        public IList<string> Claims { get; set; }
    }

    public class RoleRequestDto : RequestParameters
    {
        public RoleRequestDto()
        {
            OrderBy = "Name";
        }

        public bool Active { get; set; } = false;
    }

    public class RoleDto
    {
        [Required(ErrorMessage = "Role Name cannot be empty"), MinLength(2), MaxLength(30)]
        public string Name { get; set; }
    }
}

