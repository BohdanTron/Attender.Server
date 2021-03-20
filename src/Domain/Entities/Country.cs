using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
    }
}