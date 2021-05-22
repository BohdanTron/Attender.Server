using System.Collections.Generic;

namespace Attender.Server.Application.Users.Commands.CreateUserCities
{
    public class CreateUserCitiesDto
    {
        public IEnumerable<int> CityIds { get; set; } = new List<int>();
    }
}
