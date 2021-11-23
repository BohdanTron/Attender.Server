using System.Collections.Generic;

namespace Attender.Server.Application.SubCategories.Commands.CreateUserSubCategories
{
    public record CategoryDto
    {
        public int Id { get; init; }
        public ICollection<int> SubCategoryIds { get; init; } = new List<int>();
        public bool BindAllSubCategories { get; init; }
    }
}
