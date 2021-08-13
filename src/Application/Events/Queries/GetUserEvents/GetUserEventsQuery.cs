using Attender.Server.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Events.Queries.GetUserEvents
{
    public record GetUserEventsQuery(int UserId) : IRequest<List<EventDto>>;

    internal class GetUserEventsHandler : IRequestHandler<GetUserEventsQuery, List<EventDto>>
    {
        private readonly IAttenderDbContext _dbContext;

        public GetUserEventsHandler(IAttenderDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<List<EventDto>> Handle(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            var userPreferences = await _dbContext.Users
                .Where(u => u.Id == query.UserId)
                .Select(u => new
                {
                    LocationIds = u.Locations.Select(l => l.Id),
                    ArtistIds = u.Artists.Select(a => a.Id),
                    SubCategoryIds = u.SubCategories.Select(s => s.Id)
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            var events = await _dbContext.Events
                .Where(e => userPreferences.LocationIds.Contains(e.LocationId) &&
                            userPreferences.ArtistIds.Contains(e.ArtistId) &&
                            userPreferences.SubCategoryIds.Contains(e.SubCategoryId))
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Date = e.Date,
                    Artist = e.Artist!.Name,
                    Location = e.Location!.Name,
                    LowestPrice = e.Tickets
                        .OrderBy(t => t.Price)
                        .Select(t => t.Price)
                        .FirstOrDefault()
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return events;
        }
    }
}
