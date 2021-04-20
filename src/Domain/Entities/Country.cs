using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public bool Supported { get; set; }
        public string Code { get; set; } = default!;
        public decimal Longitude { get; set; }
        public decimal  Latitude { get; set; }
        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
    }
}