namespace Attender.Server.Application.Common.Auth.Models
{
    public record AuthTokens
    {
        public AuthTokens(Token accessToken, Token? refreshToken = null)
            => (AccessToken, RefreshToken) = (accessToken, refreshToken);

        public Token AccessToken { get; }
        public Token? RefreshToken { get; }
    }
}
