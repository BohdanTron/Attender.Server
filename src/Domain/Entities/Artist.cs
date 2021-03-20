using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}