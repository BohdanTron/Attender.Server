using System;

namespace Attender.Server.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime? OrderedDate { get; set; }
        public int? UserId { get; set; }
        public int EventId { get; set; }

        public virtual Event? Event { get; set; }
        public virtual User? User { get; set; }
    }
}