using System.Collections.Generic;

namespace Attender.Server.Application.SubCategories.Commands.CreateUserSubCategories
{
    public record CreateUserSubCategoriesDto
    {
        public ICollection<int> SubCategoryIds { get; init; } = new List<int>();
    }
}
