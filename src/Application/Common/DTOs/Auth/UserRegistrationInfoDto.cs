using System;

namespace Attender.Server.Application.Common.DTOs.Auth
{
    public record UserRegistrationInfoDto
    {
        public string UserName { get; init; } = null!;
        public string PhoneNumber { get; init; } = null!;
        public string? Email { get; init; }
        public Guid? AvatarId { get; init; }
    }
}
