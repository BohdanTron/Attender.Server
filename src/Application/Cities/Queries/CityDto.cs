using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.Cities.Queries
{
    public record CityDto : IMapFrom<City>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        
        public int CountryId { get; set; }

    }
}
