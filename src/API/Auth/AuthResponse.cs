using Attender.Server.Application.Common.Models;

namespace Attender.Server.API.Auth
{
    public class AuthResponse
    {
        public AccessToken? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
