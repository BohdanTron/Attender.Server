namespace Attender.Server.Infrastructure.Sms
{
    public record TwilioOptions
    {
        public string AccountSid { get; init; } = null!;
        public string AuthToken { get; init; } = null!;
        public string VerificationServiceSid { get; init; } = null!;
    }
}
