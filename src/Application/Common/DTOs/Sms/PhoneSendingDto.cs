namespace Attender.Server.Application.Common.Dtos.Sms
{
    public record PhoneSendingDto
    {
        public string PhoneNumber { get; init; } = null!;
    }
}
