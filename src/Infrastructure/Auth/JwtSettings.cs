namespace Attender.Server.Infrastructure.Auth
{
    public class JwtSettings
    {
        public string? SecurityKey { get; set; }
        public int LifetimeMinutes { get; set; }
        public int RefreshTokenLifetimeYears { get; set; }
    }
}
