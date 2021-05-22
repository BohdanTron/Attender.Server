namespace Attender.Server.Application.Common.Sms.Dtos
{
    public record PhoneVerificationDto
    {
        public string PhoneNumber { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
    }
}
