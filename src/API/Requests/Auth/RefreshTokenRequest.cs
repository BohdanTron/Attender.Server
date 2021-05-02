using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Requests.Auth
{
    public record RefreshTokenRequest
    {
        [Required] 
        public string AccessToken { get; init; } = null!;

        [Required]
        public string RefreshToken { get; init; } = null!;
    }
}
