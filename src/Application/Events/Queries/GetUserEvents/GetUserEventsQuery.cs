using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Mappings;
using Attender.Server.Application.Common.Models;
using Attender.Server.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public GetUserEventsHandler(IAttenderDbContext dbContext) =>
            _dbContext = dbContext;

        public Task<PaginatedList<EventDto>> Handle(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            return query.SectionId switch
            {
                (byte)EventSection.EventsForYou => GetEventsForYou(query, cancellationToken),
                (byte)EventSection.OurRecommendation => GetRecomendedEvents(query, cancellationToken),
                (byte)EventSection.Bestsellers => throw new NotImplementedException(),
                (byte)EventSection.LastChance => GetLastChanceEvents(query, cancellationToken)
                //=> throw new NotImplementedException(),
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

        private async Task<PaginatedList<EventDto>> GetLastChanceEvents(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            //var unsolvedTickets = _dbContext.Tickets
            //    .Where(t => !t.OrderedDate.HasValue)
            //    .GroupBy(t => t.EventId);
            //.Select(t=> new TicketDto
            //{
            //    Id = t.Id,
            //    EventId = t.EventId
            //});


            var unsolvedTickets2 = _dbContext.Tickets
                                    .Where(t => !t.OrderedDate.HasValue)
                                    .GroupBy(t => t.EventId)
                                    .Select(a => new
                                    {
                                        EventId = a.Key,
                                        TicetsCount = a.Count(),

                                    });

            var allTickets = _dbContext.Tickets
                                     .GroupBy(t => t.EventId)
                                    .Select(a => new
                                    {
                                        EventId = a.Key,
                                        TicetsCount = a.Count()
                                    });


            var events = await _dbContext.Events
                // .Where(t => unsolvedTickets2.Contains(t.Id))
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

            return events;
        }
    }
}
