using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public byte RoleId { get; set; }
    }
}
