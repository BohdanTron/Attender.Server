using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; }
    }
}
