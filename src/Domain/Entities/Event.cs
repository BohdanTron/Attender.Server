using System;
using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public int LocationId { get; set; }
        public int ArtistId { get; set; }
        public int SubCategoryId { get; set; }
        public bool Promoted { get; set; }
        public string? ImageURL { get; set; }
        public virtual Artist? Artist { get; set; }
        public virtual Location? Location { get; set; }
        public virtual SubCategory? SubCategory { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}