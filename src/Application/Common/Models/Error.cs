namespace Attender.Server.Application.Common.Models
{
    public record Error
    {
        public Error(string code, string message)
            => (Code, Message) = (code, message);

        public string Code { get; }
        public string Message { get; }
    }
}
