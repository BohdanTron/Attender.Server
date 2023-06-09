﻿namespace Attender.Server.Application.Common.Auth.Dtos
{
    public record UserRegistrationInfoDto
    {
        public string UserName { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string? AvatarUrl { get; init; }
        public string LanguageCode { get; init; } = string.Empty;
    }
}
