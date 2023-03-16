namespace ToDoList.Models.Enums
{
    public enum EmailTemplatePath
    {
        Default = 1,
        TwoFactorAuthentication = 2
    }

    public static class EmailTemplatePathExtension
    {
        public static string GetStringValue(this EmailTemplatePath emailTemplatePath)
        {
            return emailTemplatePath switch
            {
                EmailTemplatePath.Default => @"html\emailTemplate.html",
                EmailTemplatePath.TwoFactorAuthentication => @"html\twoFactorAuthenticationTemplate.html",
                _ => null
            };
        }
    }
}
