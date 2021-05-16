using Attender.Server.Application.Common.Mappings;
using Attender.Server.Application.SubCategories.Queries;
using Attender.Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.Categories
{
    public record CategoryDto : IMapFrom<Category>
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;

        public List<SubCategoryDto> SubCategories { get; init; } = null!;
    }
}
