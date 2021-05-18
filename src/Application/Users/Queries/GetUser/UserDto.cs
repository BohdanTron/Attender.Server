using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using System;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string UserName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public Guid? AvatarId { get; set; }
    }
}
