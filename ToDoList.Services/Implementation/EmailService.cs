using ToDoList.Models.Entities;
using ToDoList.Services.Extensions;
using ToDoList.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ToDoList.Models.Dtos.Request;
using ToDoList.Models.Dtos.Response;
using ToDoList.Models.Entities;
using ToDoList.Models.Enums;
//using ToDoList.Services.Exceptions;
using ToDoList.Services.Extensions;
using ToDoList.Services.Infrastructure;
using ToDoList.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using ToDoList.Services.Infrastructure.Email;

namespace ToDoList.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly AppConstants _constants;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string? _userId;
        private readonly IWebHostEnvironment _env;

        public EmailService(IServiceFactory serviceFactory, AppConstants constants, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _serviceFactory = serviceFactory;
            _constants = constants;
            _env = env;
            _userManager = userManager;
            _contextAccessor = _serviceFactory.GetService<IHttpContextAccessor>();
            _userId = _contextAccessor.HttpContext?.User.GetUserId();
        }


        public async Task<string> SendChangeEmail(ChangeEmailRequest request)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(_userId);
            string changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

            string path = EmailTemplatePath.Default.GetStringValue();

            EmailRequest emailRequest = EmailConstants();
            emailRequest.HtmlPath = Path.Combine(_env.WebRootPath, path);
            emailRequest.FirstName = user.FirstName;
            emailRequest.To = user.RecoveryMail;
            emailRequest.RecoveryEmail = user.RecoveryMail;
            emailRequest.NewEmail = request.NewEmail;
            emailRequest.ChangeEmailToken = changeEmailToken;

            EmailDataRequest emailData = EmailExtension.ChangeEmailData(emailRequest);

            await SendMail(emailData);

            return "A link will be sent to you if we find an account associated with this email";
        }

        public async Task SendTwoFactorAuthenticationEmail(ApplicationUser request)
        {

            string twoFactorAuthenticationToken = await _userManager.GenerateTwoFactorTokenAsync(request, _userManager.Options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultEmailProvider);

            string path = EmailTemplatePath.TwoFactorAuthentication.GetStringValue();

            EmailRequest emailRequest = EmailConstants();
            emailRequest.To = request.Email;
            emailRequest.TwoFactorAuthenticationToken = twoFactorAuthenticationToken;
            emailRequest.HtmlPath = Path.Combine(_env.WebRootPath, path);
            emailRequest.FirstName = request.FirstName;

            EmailDataRequest emailData = EmailExtension.TwoFactorAuthenticationEmailData(emailRequest);

            await SendMail(emailData);
        }

        public async Task SendCreateUserEmail(UserMailRequest request)
        {
            string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(request.User);
            string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(request.User);

            string path = EmailTemplatePath.Default.GetStringValue();

            EmailRequest emailRequest = EmailConstants();
            emailRequest.UserName = request.User.UserName;
            emailRequest.FirstName = request.FirstName;
            emailRequest.To = request.User.Email;
            emailRequest.HtmlPath = Path.Combine(_env.WebRootPath, path);
            emailRequest.EmailConfirmationToken = emailConfirmationToken;
            emailRequest.ResetPasswordToken = resetPasswordToken;

            EmailDataRequest emailData = EmailExtension.CreateUserEmailData(emailRequest);
            await SendMail(emailData);
        }

        public async Task<string> SendResetPasswordEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return string.Empty;

            string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            string path = EmailTemplatePath.Default.GetStringValue();

            EmailRequest emailRequest = EmailConstants();
            emailRequest.ResetPasswordToken = resetPasswordToken;
            emailRequest.FirstName = user.FirstName;
            emailRequest.To = email;
            emailRequest.Email = email;
            emailRequest.HtmlPath = Path.Combine(_env.WebRootPath, path);

            EmailDataRequest emailData = EmailExtension.ResetPasswordEmailData(emailRequest);

            await SendMail(emailData);

            return "A link to reset your password has been sent to your email";
        }

        public async Task SendJupebApplicationEmail(ApplicationEmailRequest request)
        {
            string path = EmailTemplatePath.Default.GetStringValue();

            EmailRequest emailRequest = EmailConstants();
            emailRequest.To = request.Email;
            emailRequest.HtmlPath = Path.Combine(_env.WebRootPath, path);
            emailRequest.Email = request.Email;
            emailRequest.FirstName = request.FirstName;
            emailRequest.ApplicationNumber = request.ApplicationNumber;
            emailRequest.ApplicationType = request.ApplicationType;
            EmailDataRequest emailData = EmailExtension.JupebApplicationEmailData(emailRequest);

            await SendMail(emailData);
        }

        public async Task<string> SendBirthdayWishEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new InvalidOperationException("Invalid email");

            if (user.Birthday != DateTime.Now.Date)
            {
                throw new InvalidOperationException($"Today is not {user.FirstName}'s birthday");
            }

            string path = EmailTemplatePath.Default.GetStringValue();

            EmailRequest emailRequest = EmailConstants();
            emailRequest.FirstName = user.FirstName;
            emailRequest.To = email;
            emailRequest.Email = email;
            emailRequest.HtmlPath = Path.Combine(_env.WebRootPath, path);

            EmailDataRequest emailData = EmailExtension.BirthdayWishEmailData(emailRequest);

            await SendMail(emailData);

            return $"A link has been sent to {email}";
        }

       
        

       

       

        
       

        
        private EmailRequest EmailConstants()
        {
            EmailRequest emailRequest = new()
            {
                From = _constants.AppEmail,
                AppURL = _constants.AppURL,
                AppAcronym = _constants.AppAcronym,
                AppName = _constants.AppAcronym,
            };

            return emailRequest;
        }

        private async Task SendMail(EmailDataRequest request)
        {
            string apiKey = _constants.SendGridAPIKey;
            SendGridClient client = new(apiKey);

            SendGridMessage newMsg = MailHelper.CreateSingleEmail(request.From, request.To, $"{_constants.AppAcronym} {request.Subject}", request.HtmlTemplate, request.HtmlTemplate);
            Response response = await client.SendEmailAsync(newMsg);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Failed to send mail");
        }
    }
}