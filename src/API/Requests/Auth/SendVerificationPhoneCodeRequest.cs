using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Requests.Auth
{
    public record SendVerificationPhoneCodeRequest
    {
        [Required]
        public string PhoneNumber { get; init; } = null!;
    }
}
