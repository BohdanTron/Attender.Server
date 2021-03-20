using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}