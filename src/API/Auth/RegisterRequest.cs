using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Auth
{
    public record RegisterRequest
    {
        [Required] 
        public string UserName { get; init; } = null!;

        [Required]
        public string PhoneNumber { get; init; } = null!;

        public string? Email { get; init; }
    }
}
