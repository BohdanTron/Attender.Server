using Attender.Server.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Attender.Server.Application.Countries.Services
{
    public class PopularCitiesService
    {
        private const int MaxCitiesCount = 5;

        private readonly IAttenderDbContext _dbContext;

        public PopularCitiesService(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ILookup<int, CityDto> Get(IEnumerable<int> countryIds)
        {
            var cities = _dbContext.Events
                .Join(_dbContext.Locations, e => e.LocationId, l => l.Id, (e, l) => new { e, l })
                .Join(_dbContext.Cities, el => el.l.CityId, c => c.Id, (el, c) => new { el, c })
                .Where(elc => countryIds.Contains(elc.c.CountryId))
                .GroupBy(elc => new { elc.c.Name, elc.c.Id, elc.c.CountryId })
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Key.CountryId)
                .Select(group => new CityDto
                {
                    Id = group.Key.Id,
                    Name = group.Key.Name,
                    CountryId = group.Key.CountryId
                })
                .Take(MaxCitiesCount)
                .ToLookup(c => c.CountryId);

            return cities;
        }
    }
}
