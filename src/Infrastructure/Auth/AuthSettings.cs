namespace Attender.Server.Infrastructure.Auth
{
    public class AuthSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string SecurityKey { get; set; } = string.Empty;
        public int LifetimeMinutes { get; set; }
        public int RefreshTokenLifetimeYears { get; set; }
    }
}
