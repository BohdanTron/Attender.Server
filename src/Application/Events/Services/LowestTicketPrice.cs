using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Tickets.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Application.Events.Services
{
    public class LowestTicketPrice
    {
        private readonly IAttenderDbContext _dbContext;

        public LowestTicketPrice(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ILookup<int, TicketDto> Get(IEnumerable<int> eventsIds)
        {
            //var cities = _dbContext.Events
            //    .Join(_dbContext.Locations, e => e.LocationId, l => l.Id, (e, l) => new { e, l })
            //    .Join(_dbContext.Cities, el => el.l.CityId, c => c.Id, (el, c) => new { el, c })
            //    .Where(elc => countryIds.Contains(elc.c.CountryId))
            //    .GroupBy(elc => new { elc.c.Name, elc.c.Id, elc.c.CountryId })
            //    .OrderByDescending(g => g.Count())
            //    .ThenByDescending(g => g.Key.CountryId)
            //    .Select(group => new CityDto
            //    {
            //        Id = group.Key.Id,
            //        Name = group.Key.Name,
            //        CountryId = group.Key.CountryId
            //    })
            //    .Take(MaxCitiesCount)
            //    .AsNoTracking()
            //    .ToLookup(c => c.CountryId);

            //return cities;

            var tickietsPrice = _dbContext.Tickets
                .Where(c => eventsIds.Contains(c.EventId))
                .OrderByDescending(p => p.Price)
                .Select(t => new TicketDto
                {
                    Price = t.Price
                })
                .Take(1)
                .AsNoTracking()
                .ToLookup(c => c.Id);

            return tickietsPrice;

                //   .Where(elc => countryIds.Contains(elc.c.CountryId))
                //.GroupBy(elc => new { elc.c.Name, elc.c.Id, elc.c.CountryId })
                //.OrderByDescending(g => g.Count())
        }
    }
}
