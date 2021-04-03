using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Auth
{
    public class VerifyPhoneRequest
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
