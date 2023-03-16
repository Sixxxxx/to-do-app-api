using ToDoList.Models.Dtos.Request;
using SendGrid.Helpers.Mail;
using System.Text;
using ToDoList.Services.Infrastructure;
using ToDoList.Services.Infrastructure.Email;
using MessageEncoder;
using ToDoList.Services.Infrastructure.Email;

namespace ToDoList.Services.Extensions
{
    public class EmailExtension
    {
        public static string GetHtmlTemplate(string path)
        {
            if (!File.Exists(path))
                return null;

            string[] lines = File.ReadAllLines(path);
            StringBuilder sb = new();

            foreach (var line in lines)
            {
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static string ReplaceHtmlPlaceHolders(EmailDataRequest request)
        {
            Dictionary<string, string> variables = new()
            {
                ["Body"] = request.Body,
                ["Url"] = request.URL,
                ["ButtonText"] = request.ButtonText,
                ["Heading"] = request.Heading,
                ["Year"] = request.Year,
                ["AppName"] = request.AppName,
            };

            for (int i = 0; i < variables.Count; i++)
            {
                string htmlPlaceHolder = variables.ElementAt(i).Key;
                string htmlText = variables.ElementAt(i).Value;

                request.HtmlTemplate = request.HtmlTemplate.Replace(htmlPlaceHolder, htmlText);
            }

            return request.HtmlTemplate;
        }

        public static EmailDataRequest JupebApplicationEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}";
            string heading = "Sub-degree Application";
            string body = $"Hello {request.FirstName}, your {request.ApplicationType} Application registration was successful and your Application Number is {request.ApplicationNumber}." +
                $"\nThe next steps will be communicated to you.";
            string buttonText = "Back to portal";
            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest TwoFactorAuthenticationEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string heading = "Two Factor Authentication";
            string body = $"Hello {request.FirstName}, \nEnter the code {request.TwoFactorAuthenticationToken} to log in to your account";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest ResetPasswordEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string encodedToken = MessageEncoder.Encoder.EncodeMessage(request.ResetPasswordToken);
            string encodedEmail = MessageEncoder.Encoder.EncodeMessage(request.Email);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}/reset_password?q={encodedEmail}&w={encodedToken}&i=rp";

            string heading = "Reset Password";
            string body = $"Hello {request.FirstName}, to reset your password click the link below. If this wasn't you, ignore this message and contact us";
            string buttonText = "Reset Password";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText,
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }
        public static EmailDataRequest CreateUserEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string encodedUsername = MessageEncoder.Encoder.EncodeMessage(request.UserName);
            string encodedEmailConfirmationToken = MessageEncoder.Encoder.EncodeMessage(request.EmailConfirmationToken);
            string encodedResetPasswordToken = MessageEncoder.Encoder.EncodeMessage(request.ResetPasswordToken);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}/reset_password?q={encodedUsername}&w={encodedEmailConfirmationToken}&e={encodedResetPasswordToken}&i=cu";
            string heading = "Verify Account";
            string body =
                $"Hello {request.FirstName}, your username is {request.UserName.ToUpper()}. To verify your account, click the link below. Note that you will need to reset your password in order to access your account";
            string buttonText = "Reset Password";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText,
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }


        public static EmailDataRequest ChangeEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string encodedChangeEmailToken = MessageEncoder.Encoder.EncodeMessage(request.ChangeEmailToken);

            string encodedEmail = MessageEncoder.Encoder.EncodeMessage(request.RecoveryEmail);
            string encodedNewEmail = MessageEncoder.Encoder.EncodeMessage(request.NewEmail);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string heading = "Change Email";
            string url = $"{request.AppURL}/change_email?q={encodedEmail}&w={encodedNewEmail}&e={encodedChangeEmailToken}&i=ce";
            string body = $"Hello {request.FirstName}, to change your email, click the link below. If this wasn't you, ignore this message and contact us.";
            string buttonText = "Change Email";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                ButtonText = buttonText,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }


        public static EmailDataRequest BirthdayWishEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);            

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}/login";
            string heading = "Happy Birthday";
            string body =
                $"Hello! {request.FirstName}\n, Your birthday is very special and we have a surprise for you in your portal, login in to your portal to view.";
            string buttonText = "View Surprise";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText,
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest MatricNumberEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string heading = "Matric Number";
            string body = $"Congratulations {request.FirstName} once again on your admission into the university. Your matric number is {request.MatricNo}.";
            string url = request.AppURL;

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = "Login"
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest PGAppEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}/pg_application_details?refcode={request.RRR}";
            string heading = "Post-Graduate Application";
            string body = $"Hello {request.ReceiverFullName}, your Post-Graduate Application registration was successful and your Application Number is {request.ApplicationNumber}." +
                          "<br>Click the button below to track your application";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = "Track Your Application",
                Subject = ": PG APPLICATION SUCCESSFUL",
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest NotifyRefereeData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string heading = $"Reference Requested for {request.SenderFullName.ToUpper()} ({request.MatricNo})";
            string url = $"{request.AppURL}/pg_referee_form?passcode={request.PassCode}";
            string body =
                $"Dear {request.ReceiverFullName},<br><br>" +
                $"This request is from {request.AppName} - School of Postgraduate Studies.<br> <span style=\"font-weight:bold\">{request.SenderFullName}</span>" +
                $" has recently made an application to the {request.AppName}, School of Postgraduate Studies," +
                $" and has named you as a referee. Please could you fill the online reference form by clicking the link " +
                $"below and the submit button to return the form to the School of Postgraduate Studies, {request.AppName}.," +
                $" click the button below. If this wasn't you, ignore this message and contact us.<br><br>If you receive more than one" +
                $" request like this, it is probable that the applicant has submitted more than one application. Each of the requests" +
                $" will have a different Application Number.<br><br>Thank you for your help!";

            string buttonText = "Complete Form";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                Subject = ": PG APPLICANT REFEREE",
                ButtonText = buttonText,
                From = from,
                To = to,
                AppName = $"School of Postgraduate Studies<br>{request.AppName}",
                HtmlTemplate = htmlTemplate,
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest CSEPGApplicationEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}";
            string heading = "Centre For Safety Education";
            string body = $"Hello {request.FirstName}, your Application registration was successful and your Application Number is {request.ApplicationNumber}." +
                $"\nThe next steps will be communicated to you.";
            string buttonText = "Back to portal";
            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest CSENDApplicationEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}";
            string heading = "Centre For Safety Education";
            string body = $"Hello {request.FirstName}, your Application registration was successful and your Application Number is {request.ApplicationNumber}." +
                $"\nThe next steps will be communicated to you.";
            string buttonText = "Back to portal";
            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }

        public static EmailDataRequest DEApplicationEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}";
            string heading = "Direct Entry Application";
            string body = $"Hello {request.FirstName}, your Direct Entry Application registration was successful and your Application Number is {request.ApplicationNumber}." +
                $"\nThe next steps will be communicated to you.";
            string buttonText = "Back to portal";
            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }


        public static EmailDataRequest ImpersonationEmailData(EmailRequest request)
        {
            EmailAddress from = new(request.From, request.AppAcronym);
            EmailAddress to = new(request.To);

            string encodedToken = MessageEncoder.Encoder.EncodeMessage(request.ImpersonationToken);
            string encodedCurrentUserId = MessageEncoder.Encoder.EncodeMessage(request.UserId);
            string encodedUserIIdToImpersonate = MessageEncoder.Encoder.EncodeMessage(request.UserIdToImpersonate);

            string htmlTemplate = GetHtmlTemplate(request.HtmlPath);

            string url = $"{request.AppURL}/impersonation_login?cuid={encodedCurrentUserId}&uidti={encodedUserIIdToImpersonate}&it={encodedToken}";

            string heading = "User Impersonation Request";
            string body = $"Hello {request.FirstName}, to impersonate {request.FirstNameToImpersonate} click the link below.<br>This link is only valid for 10 minutes. If this wasn't you, ignore this message and contact us.";
            string buttonText = "Impersonate";

            EmailDataRequest emailData = new()
            {
                Heading = heading,
                Body = body,
                URL = url,
                From = from,
                To = to,
                AppName = request.AppName,
                HtmlTemplate = htmlTemplate,
                ButtonText = buttonText,
            };

            emailData.HtmlTemplate = ReplaceHtmlPlaceHolders(emailData);
            return emailData;
        }
    }
}