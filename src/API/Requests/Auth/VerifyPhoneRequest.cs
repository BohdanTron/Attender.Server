using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Requests.Auth
{
    public record VerifyPhoneRequest
    {
        [Required]
        public string PhoneNumber { get; init; } = null!;

        [Required]
        public string Code { get; init; } = null!;
    }
}
