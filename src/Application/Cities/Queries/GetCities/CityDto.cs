using Attender.Server.Application.Common.Mappings;
using Attender.Server.Domain.Entities;

namespace Attender.Server.Application.Cities.Queries.GetCities
{
    public record CityDto : IMapFrom<City>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int CountryId { get; init; }
    }
}
