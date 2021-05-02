using System;
using System.ComponentModel.DataAnnotations;

namespace Attender.Server.API.Requests.Auth
{
    public record RegisterRequest
    {
        [Required]
        [StringLength(25)]
        public string UserName { get; init; } = null!;

        [Required]
        [RegularExpression("^\\+[1-9]{1}[0-9]{7,14}$")]
        public string PhoneNumber { get; init; } = null!;

        [EmailAddress]
        [StringLength(50)]
        public string? Email { get; init; }

        public Guid? AvatarId { get; init; }
    }
}
