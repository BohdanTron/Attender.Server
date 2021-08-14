using System.Collections.Generic;

namespace Attender.Server.Application.Cities.Commands.CreateUserCities
{
    public class CreateUserCitiesDto
    {
        public ICollection<int> CityIds { get; set; } = new List<int>();
    }
}
