using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Mappings;
using Attender.Server.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EventSection = Attender.Server.Domain.Enums.EventSection;

namespace Attender.Server.Application.Events.Queries.GetUserEvents
{
    public record GetUserEventsQuery : IRequest<PaginatedList<EventDto>>
    {
        public int UserId { get; init; }
        public int SectionId { get; init; }
        public int PageSize { get; init; } = 1;
        public int PageNumber { get; init; } = 10;
    }

    internal class GetUserEventsHandler : IRequestHandler<GetUserEventsQuery, PaginatedList<EventDto>>
    {
        private readonly IAttenderDbContext _dbContext;

        public GetUserEventsHandler(IAttenderDbContext dbContext) => _dbContext = dbContext;

        public Task<PaginatedList<EventDto>> Handle(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            return query.SectionId switch
            {
                (byte)EventSection.EventsForYou => GetEventsForYou(query, cancellationToken),
                (byte)EventSection.OurRecommendation => GetRecomendedEvents(query, cancellationToken),
                (byte)EventSection.Bestsellers => GetBestsellersEvents(query, cancellationToken),
                (byte)EventSection.LastChance => GetLastChanceEvents(query, cancellationToken),
                _ => throw new NotImplementedException()
            };
        }

        private async Task<PaginatedList<EventDto>> GetEventsForYou(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            var userPreferences = await _dbContext.Users
                .Where(u => u.Id == query.UserId)
                .Select(u => new
                {
                    LocationIds = u.Locations.Select(l => l.Id),
                    ArtistIds = u.Artists.Select(a => a.Id),
                    SubCategoryIds = u.SubCategories.Select(s => s.Id)
                })
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
                .ToPaginatedListAsync(query.PageSize, query.PageNumber, cancellationToken);

            return events;
        }

        private async Task<PaginatedList<EventDto>> GetRecomendedEvents(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            var events = await _dbContext.Events
                .Where(e => e.Promoted)
                .Select(ev => new EventDto
                {
                    Id = ev.Id,
                    Name = ev.Name,
                    Description = ev.Description,
                    Artist = ev.Artist!.Name,
                    Location = ev.Location!.Name,
                    LowestPrice = ev.Tickets
                        .OrderBy(t => t.Price)
                        .Select(t => t.Price)
                        .FirstOrDefault()

                })
             .ToPaginatedListAsync(query.PageSize, query.PageNumber, cancellationToken);

            return events;
        }

        private async Task<PaginatedList<EventDto>> GetLastChanceEvents(GetUserEventsQuery query,
            CancellationToken cancellationToken)
        {
            var events = await _dbContext.Events
                .Select(e => new
                {
                    EventId = e.Id,
                    SoldTicketsCount = e.Tickets.Count(t => t.OrderedDate != null),
                    UnsoldTicketsCount = e.Tickets.Count(t => t.OrderedDate == null)
                }).ToListAsync(cancellationToken);

            var eventIds = events
                .Where(e => e.UnsoldTicketsCount != 0 && e.SoldTicketsCount / e.UnsoldTicketsCount < 0.2)
                .Select(e => e.EventId)
                .ToList();

            var lastChanceEvents = await _dbContext.Events
                .Where(t => eventIds.Contains(t.Id))
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Artist = e.Artist!.Name,
                    Location = e.Location!.Name,
                    LowestPrice = e.Tickets
                        .OrderBy(t => t.Price)
                        .Select(t => t.Price)
                        .FirstOrDefault()

                })
                .ToPaginatedListAsync(query.PageSize, query.PageNumber, cancellationToken);

            return lastChanceEvents;
        }

        private async Task<PaginatedList<EventDto>> GetBestsellersEvents(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            var events = await _dbContext.Events
                .Select(e => new
                {
                    EventId = e.Id,
                    SoldTicketsCount = e.Tickets.Count(t => t.OrderedDate != null),
                    StartSalesDate = e.Tickets.OrderByDescending(t => t.OrderedDate)
                    .Select(t => t.OrderedDate).SingleOrDefault()
                                               
                }).ToListAsync(cancellationToken);

            var eventIds = events
                .Where(e => e.SoldTicketsCount != 0)
                .OrderByDescending(e => e.SoldTicketsCount / (DateTime.Now.Second - e.StartSalesDate.Value.Second))
                .Select(e => e.EventId)
                .ToList();

            var bestsellersEvents = await _dbContext.Events
                .Where(t => eventIds.Contains(t.Id))
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Artist = e.Artist!.Name,
                    Location = e.Location!.Name,
                    LowestPrice = e.Tickets
                        .OrderBy(t => t.Price)
                        .Select(t => t.Price)
                        .FirstOrDefault()

                })
                .ToPaginatedListAsync(query.PageSize, query.PageNumber, cancellationToken);

            return bestsellersEvents;
        }
    }
}
