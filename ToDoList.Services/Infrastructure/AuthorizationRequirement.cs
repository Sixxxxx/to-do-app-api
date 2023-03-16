using Microsoft.AspNetCore.Authorization;

namespace ToDoList.Services.Infrastructure
{
    public class AuthorizationRequirment : IAuthorizationRequirement
    {
        public int Success { get; set; }
    }
}
