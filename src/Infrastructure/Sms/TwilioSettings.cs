namespace Attender.Server.Infrastructure.Sms
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string VerificationServiceSid { get; set; } = string.Empty;
    }
}
