using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Auth
{
    public record SendVerificationPhoneCodeRequest
    {
        //TODO: Add phone format validation
        [Required]
        public string PhoneNumber { get; init; } = null!;
    }
}
