using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;

        public virtual ICollection<User> Users { get; set; } =
            new HashSet<User>();

        public virtual ICollection<CategoryDescription> CategoryDescriptions { get; set; } =
            new HashSet<CategoryDescription>();
    }
}