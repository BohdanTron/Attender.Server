namespace Attender.Server.Application.Common.Sms.Dtos
{
    public record PhoneSendingDto
    {
        public string PhoneNumber { get; init; } = string.Empty;
    }
}
