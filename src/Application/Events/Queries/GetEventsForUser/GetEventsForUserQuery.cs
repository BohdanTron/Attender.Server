using Attender.Server.Application.Common.Interfaces;
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
    public record GetEventsForUserQuery(int Id) : IRequest<List<EventDto>>;
    
    internal class GetEventsForUserHandler: IRequestHandler<GetEventsForUserQuery, List<EventDto>>
    {
        private readonly IAttenderDbContext _dbContext;

        private readonly IMapper _mapper;


        public GetEventsForUserHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<EventDto>> Handle(GetEventsForUserQuery query, CancellationToken cancellationToken)
        {
            var events = await _dbContext.Events
                .Where(e => e.Users.Any(u => u.Id == query.Id && u.Locations.Any() && u.SubCategories.Any()))
                .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);


            var events2 = await _dbContext.Users
                .Where(u => u.Id == query.Id && u.SubCategories.Any() && u.Locations.Any())
                .SelectMany(u => u.Events.Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name
                }))
                .ToListAsync(cancellationToken);

            return events;
        }
    }
}
