using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public virtual ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
    }
}