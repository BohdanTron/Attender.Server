using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int CityId { get; set; }

        public virtual City? City { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}