using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Cities.Queries.GetCities
{
    public class GetCitiesQuery : IRequest<List<CityDto>>
    {
        public string? Name { get; set; }
    }

    internal class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCitiesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<CityDto>> Handle(GetCitiesQuery query, CancellationToken cancellationToken)
        {
            if (query.Name is null)
                return Task.FromResult(Enumerable.Empty<CityDto>().ToList());

            return _dbContext.Cities
                .AsNoTracking()
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                .Where(c => c.Name.Contains(query.Name))
                .ToListAsync(cancellationToken);
        }
    }
}
