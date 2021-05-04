using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.SubCategories.Queries
{
    public record SubCategoryDto : IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public int CategoryId { get; set; }
    }
}
