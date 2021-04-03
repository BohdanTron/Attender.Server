using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Auth
{
    public class SendVerificationPhoneCodeRequest
    {
        //TODO: Add phone format validation
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
