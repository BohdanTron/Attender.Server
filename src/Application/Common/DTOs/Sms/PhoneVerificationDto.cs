namespace Attender.Server.Application.Common.Dtos.Sms
{
    public record PhoneVerificationDto
    {
        public string PhoneNumber { get; init; } = null!;
        public string Code { get; init; } = null!;
    }
}
