using System.Collections.Generic;

namespace Attender.Server.Application.Cities.Commands.CreateUserCities
{
    public record CreateUserCitiesDto
    {
        public ICollection<CountryDto> Countries { get; init; } = new List<CountryDto>();
        public bool BindAllCountries { get; init; }
    }
}
