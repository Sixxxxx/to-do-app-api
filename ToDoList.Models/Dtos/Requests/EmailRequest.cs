using ToDoList.Models.Entities;

public class UserMailRequest
{
    public ApplicationUser User { get; set; }
    public string FirstName { get; set; }

}

public class EmailRequest : ApplicationEmailRequest
{
    public string FirstName { get; set; }
    public string UserName { get; set; }
    public string AppURL { get; set; }
    public string AppAcronym { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string HtmlPath { get; set; }
    public string AppName { get; set; }
    public string MatricNo { get; set; }
    public string TwoFactorAuthenticationToken { get; set; }
    public string ResetPasswordToken { get; set; }
    public string Email { get; set; }
    public string NewEmail { get; set; }
    public string RecoveryEmail { get; set; }
    public string ChangeEmailToken { get; set; }
    public string EmailConfirmationToken { get; set; }
    public string PassCode { get; set; }
    public string SenderFullName { get; set; }
    public string ReceiverFullName { get; set; }
    public string RRR { get; set; }
    public string ImpersonationToken { get; set; }
    public string UserId { get; set; }
    public string UserIdToImpersonate { get; set; }
    public string FirstNameToImpersonate { get; set; }
}

public class ApplicationEmailRequest
{
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string ApplicationNumber { get; set; }
    public string ApplicationType { get; set; }
}