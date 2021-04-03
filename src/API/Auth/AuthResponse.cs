using Attender.Server.Application.Common.Models;

namespace Attender.Server.API.Auth
{
    public class AuthResponse
    {
        public Token AccessToken { get; set; } = default!;

        public Token? RefreshToken { get; set; }
    }
}
