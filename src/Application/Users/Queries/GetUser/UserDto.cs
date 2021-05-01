using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public class UserDto : IMapFrom<User>
    {
        [Required]
        public int Id { get; set; }

        public string? Email { get; set; }

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public Guid? AvatarId { get; set; }
    }
}
