using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual ICollection<Location> Locations { get; set; } = new HashSet<Location>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}