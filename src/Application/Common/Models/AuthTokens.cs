namespace Attender.Server.Application.Common.Models
{
    public record AuthTokens (Token AccessToken, Token? RefreshToken = null);
}
