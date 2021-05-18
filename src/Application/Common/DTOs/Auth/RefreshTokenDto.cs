namespace Attender.Server.Application.Common.DTOs.Auth
{
    public record RefreshTokenDto
    {
        public string AccessToken { get; init; } = null!;
        public string RefreshToken { get; init; } = null!;
    }
}
