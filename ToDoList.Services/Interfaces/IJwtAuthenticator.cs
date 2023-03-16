using ToDoList.Models.Dtos.Response;
using ToDoList.Models.Entities;
using System.Security.Claims;

namespace ToDoList.Services.Interfaces
{
    public interface IJWTAuthenticator
    {
        Task<JwtToken> GenerateJwtToken(ApplicationUser user, string expires = null, List<Claim> additionalClaims = null);
    }
}
