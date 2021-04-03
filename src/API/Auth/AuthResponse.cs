using Attender.Server.Application.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Auth
{
    public class AuthResponse
    {
        [Required]
        public AccessToken AccessToken { get; set; } = default!;

        public string? RefreshToken { get; set; }
    }
}
