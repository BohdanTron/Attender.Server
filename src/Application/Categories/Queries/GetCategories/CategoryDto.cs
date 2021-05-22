using Attender.Server.Application.Common.Mappings;
using Attender.Server.Application.SubCategories.Queries.GetSubCategories;
using Attender.Server.Domain.Entities;
using System.Collections.Generic;

namespace Attender.Server.Application.Categories.Queries.GetCategories
{
    public record CategoryDto : IMapFrom<Category>
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public IEnumerable<SubCategoryDto> SubCategories { get; init; } = new List<SubCategoryDto>();
    }
}
