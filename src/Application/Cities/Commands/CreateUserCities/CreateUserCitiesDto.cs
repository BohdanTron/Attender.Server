using System.Collections.Generic;

namespace Attender.Server.Application.Cities.Commands.CreateUserCities
{
    public record CreateUserCitiesDto
    {
        public ICollection<CountryDto> Countries { get; set; } = new List<CountryDto>();
        public bool BindAllCountries { get; set; }
    }
}
