using System;
using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid? AvatarId { get; set; }
        public byte RoleId { get; set; }
        public int LanguageId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual Language? Language { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<Artist> Artists { get; set; } = new HashSet<Artist>();
        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
        public virtual ICollection<Location> Locations { get; set; } = new HashSet<Location>();
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
    }
}