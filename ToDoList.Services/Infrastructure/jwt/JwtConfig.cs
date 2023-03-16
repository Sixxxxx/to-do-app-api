
namespace ToDoList.Services.Infrastructures.jwt
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string Expires { get; set; }
        public string ImpersonationExpires { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
