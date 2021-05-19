using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;

namespace Attender.Server.Application.SubCategories.Queries.GetSubCategories
{
    public record SubCategoryDto : IMapFrom<SubCategory>
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public int CategoryId { get; init; }
    }
}
