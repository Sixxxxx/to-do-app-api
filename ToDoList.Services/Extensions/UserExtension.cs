using System.Security.Claims;

namespace ToDoList.Services.Extensions
{
    public static class UserExtension
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst("Id")?.Value;
        }
    }
}
