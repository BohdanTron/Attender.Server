using System.Collections.Generic;

namespace Attender.Server.Application.Categories.Queries.GetCategories
{
    public record CategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public IEnumerable<SubCategoryDto> SubCategories { get; init; } = new List<SubCategoryDto>();
    }

    public record SubCategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int CategoryId { get; init; }
    }
}
