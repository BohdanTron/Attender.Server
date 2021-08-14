using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Cities.Queries.GetCities
{
    public record GetCitiesQuery(string? Name) : IRequest<List<CityDto>>;

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
            return _dbContext.Cities
                .Where(CitiesPredicate(query))
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        private static Expression<Func<City, bool>> CitiesPredicate(GetCitiesQuery query)
        {
            return c => query.Name == null
                ? c.Country!.Supported
                : c.Country!.Supported && c.Name.Contains(query.Name);
        }
    }
}
