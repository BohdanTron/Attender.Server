using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Role
    {
        public byte Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}