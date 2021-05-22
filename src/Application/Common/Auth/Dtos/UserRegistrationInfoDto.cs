using System;

namespace Attender.Server.Application.Common.Auth.Dtos
{
    public record UserRegistrationInfoDto
    {
        public string UserName { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string? Email { get; init; }
        public Guid? AvatarId { get; init; }
    }
}
