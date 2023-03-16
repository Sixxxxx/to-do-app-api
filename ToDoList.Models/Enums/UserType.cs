namespace ToDoList.Models.Enums;

public enum UserType
{
    Staff = 1,
    Admin,
}

public static class UserTypeExtension
{
    public static string? GetStringValue(this UserType userType)
    {
        return userType switch
        {
            UserType.Staff => "Staff",
            UserType.Admin => "Admin",
            _ => null
        };
    }
}