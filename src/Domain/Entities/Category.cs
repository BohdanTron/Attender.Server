using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();

        public virtual ICollection<CategoryDescription> CategoryDescriptions { get; set; } =
            new HashSet<CategoryDescription>();
    }
}