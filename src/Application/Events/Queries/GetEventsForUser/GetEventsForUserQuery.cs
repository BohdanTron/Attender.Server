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
                //.Join(_dbContext.Locations, e => e.LocationId, l => l.Id, (e, l) => new { e, l })
                //.Join(_dbContext.SubCategories, es => es.e.SubCategoryId, sb => sb.Id, (es, sb) => new { es, sb })
                //.Join(_dbContext.Users, us => us.sb.Id, u => u.Id, (us, u) => new { us, u })
                // .Where(a => a.u.Id.Equals(query.Id))
                // .Select(events => new EventDto
                // {
                //     Id = events.us.es.e.Id,
                //     Name = events.us.es.e.Name,
                //     Description = events.us.es.e.Description
                // })
                .Include(l => l.Location)
                .Include(usb => usb.SubCategory)
                .Include(u => u.Users)
               // .Where(a => a.Users.Select(x => x.Id).Equals(query.Id))
                .Where(a => a.Id.Equals(query.Id))
                ////.Select(e => new EventDto
                ////{
                ////    Id = e.Id,
                ////    Name = e.Name,
                ////    Description = e.Description
                ////})
                 .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return events;
        }
    }
}
