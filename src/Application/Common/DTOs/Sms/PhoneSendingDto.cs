namespace Attender.Server.Application.Common.DTOs.Sms
{
    public record PhoneSendingDto
    {
        public string PhoneNumber { get; init; } = null!;
    }
}
