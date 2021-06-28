namespace Attender.Server.Infrastructure.Auth
{
    public record AuthOptions
    {
        public string Issuer { get; init; } = null!;
        public string Key { get; init; } = null!;
        public int LifetimeMinutes { get; init; }
        public int RefreshTokenLifetimeYears { get; init; }
    }
}
