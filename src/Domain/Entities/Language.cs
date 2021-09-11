using System.Collections.Generic;

namespace Attender.Server.Domain.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public virtual ICollection<EventSection> EventSections { get; set; } = new HashSet<EventSection>();

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

        public virtual ICollection<CategoryDescription> CategoryDescriptions { get; set; } =
            new HashSet<CategoryDescription>();
    }
}