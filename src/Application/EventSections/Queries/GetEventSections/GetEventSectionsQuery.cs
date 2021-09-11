using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.EventSections.Queries.GetEventSections
{
    public record GetEventSectionsQuery(int UserId) : IRequest<List<EventSectionDto>>;

    internal class GetEventSectionsQueryHandler : IRequestHandler<GetEventSectionsQuery, List<EventSectionDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetEventSectionsQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<EventSectionDto>> Handle(GetEventSectionsQuery request, CancellationToken cancellationToken)
        {
            var userLanguageId = await _dbContext.Users
                .Where(u => u.Id == request.UserId)
                .Select(u => u.LanguageId)
                .FirstAsync(cancellationToken);

            var eventSections = await _dbContext.EventSections
                .Where(e => e.LanguageId == userLanguageId)
                .ProjectTo<EventSectionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return eventSections;
        }
    }
}
