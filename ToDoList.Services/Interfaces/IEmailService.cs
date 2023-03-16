using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Entities;

namespace ToDoList.Services.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendChangeEmail(ChangeEmailRequest request);
        Task SendTwoFactorAuthenticationEmail(ApplicationUser user);
        Task SendCreateUserEmail(UserMailRequest request);
        Task<string> SendResetPasswordEmail(string email);
    }
}
