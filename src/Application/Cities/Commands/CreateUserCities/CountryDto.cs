using System.Collections.Generic;

namespace Attender.Server.Application.Cities.Commands.CreateUserCities
{
    public record CountryDto
    {
        public int Id { get; init; }
        public ICollection<int> CityIds { get; init; } = new List<int>();
        public bool BindAllCities { get; init; }
    }
}
