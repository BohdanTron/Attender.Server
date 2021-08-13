using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Events.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Events.Queries.GetEventsForUser
{
    public record GetUserEventsQuery(int Id) : IRequest<List<EventDto>>;
    
    internal class GetEventsForUserHandler: IRequestHandler<GetUserEventsQuery, List<EventDto>>
    {
        private readonly IAttenderDbContext _dbContext;

        private readonly IMapper _mapper;

        private readonly EventsService _eventsService;


        public GetEventsForUserHandler(IAttenderDbContext dbContext, IMapper mapper, EventsService eventsService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _eventsService = eventsService;
        }

        public async Task<List<EventDto>> Handle(GetUserEventsQuery query, CancellationToken cancellationToken)
        {
            var userLocationIds = await _dbContext.Users
               .Where(u => u.Id == query.Id && u.Locations.Any())
               .SelectMany(ul => ul.Locations.Select(l => l.Id))
               .ToListAsync(cancellationToken);

            var userArtistsIds = await _dbContext.Users
                .Where(u => u.Id == query.Id && u.Artists.Any())
                .SelectMany(ua => ua.Artists.Select(a => a.Id))
                .ToListAsync(cancellationToken);

            var userSubCategoriesIds = await _dbContext.Users
                .Where(u => u.Id == query.Id && u.SubCategories.Any())
                .SelectMany(us => us.SubCategories.Select(s => s.Id))
                .ToListAsync(cancellationToken);


            var events = await _dbContext.Events
                .Where(e => userLocationIds.Contains(e.LocationId)
                        && userArtistsIds.Contains(e.ArtistId)
                        && userSubCategoriesIds.Contains(e.SubCategoryId))
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .OrderBy(e => e.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken);


            return _eventsService.GetEventsWithTicketPrice(events);
        }
    }
}
