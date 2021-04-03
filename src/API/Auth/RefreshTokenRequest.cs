﻿using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Auth
{
    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
