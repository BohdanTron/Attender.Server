using System.Collections.Generic;

namespace Attender.Server.Application.SubCategories.Commands.CreateUserSubCategories
{
    public record CreateUserSubCategoriesDto
    {
        public ICollection<CategoryDto> Categories { get; init; } = new List<CategoryDto>();
        public bool BindAllCategories { get; init; }
    }
}
