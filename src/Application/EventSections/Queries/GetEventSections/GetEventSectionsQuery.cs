using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.EventSections.Queries.GetEventSections
{
    public record GetEventSectionsQuery : IRequest<List<EventSectionDto>>;

    internal class GetEventSectionsQueryHandler : IRequestHandler<GetEventSectionsQuery, List<EventSectionDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetEventSectionsQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<EventSectionDto>> Handle(GetEventSectionsQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.EventSections
                .ProjectTo<EventSectionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
