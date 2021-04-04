using Attender.Server.Application.Common.Models;

namespace Attender.Server.API.Auth
{
    public class AuthResponse
    {
        public AuthResponse(Token accessToken, Token? refreshToken = null) 
            => (AccessToken, RefreshToken) = (accessToken, refreshToken);

        public Token AccessToken { get; }

        public Token? RefreshToken { get; }
    }
}
