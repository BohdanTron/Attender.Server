using Attender.Server.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Attender.Server.Application.Events.Services
{
    public class EventsService
    {
        private readonly IAttenderDbContext _dbContext;

        public EventsService(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<EventDto> GetEventsWithTicketPrice(List<EventDto> events)
        {
            foreach (var e in events)
            {
                var tickietsPrice = _dbContext.Tickets
                       .Where(t => t.EventId.Equals(e.Id))
                       .OrderBy(p => p.Price)
                       .Select(t => new TicketDto
                       {
                           Price = t.Price
                       })
                       .Take(1)
                       .AsNoTracking()
                       .FirstOrDefault();

                e.LowestTicketPrice = tickietsPrice?.Price;
            }

            return events;
        }
    }
}
