namespace ToDoList.Models.Dtos.Response;

public class AuthenticationResponse
{
    public JwtToken JwtToken { get; set; }
    public string UserType { get; set; }
    public string FullName { get; set; }
    public IEnumerable<string> MenuItems { get; set; }
    public bool? Birthday { get; set; }
    public bool TwoFactor { get; set; }
    public string UserId { get; set; }
    public string ImpersonatorUsername { get; set; }
    public bool IsImpersonating { get; set; }
}

public class JwtToken
{
    public string Token { get; set; }
    public DateTime Issued { get; set; }
    public DateTime? Expires { get; set; }
}