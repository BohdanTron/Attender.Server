namespace Attender.Server.Application.Common.Auth.Dtos
{
    public record UserDto
    {
        public int Id { get; init; }
        public string? Email { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string? AvatarUrl { get; init; }
    }
}
