namespace Attender.Server.Application.Common.Models
{
    public class AuthTokens
    {
        public AuthTokens(Token accessToken, Token? refreshToken = null)
            => (AccessToken, RefreshToken) = (accessToken, refreshToken);

        public Token AccessToken { get; }

        public Token? RefreshToken { get; }
    }
}
