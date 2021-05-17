using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;

namespace Attender.Server.Application.Cities.Queries
{
    public record CityDto : IMapFrom<City>
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;
        
        public int CountryId { get; init; }
    }
}
