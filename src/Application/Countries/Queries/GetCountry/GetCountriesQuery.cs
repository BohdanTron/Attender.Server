using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Queries.GetCountry
{
    public class GetCountriesQuery : IRequest<List<CountryDto>>
    {
        public GetCountriesQuery(string name) => Name = name;

        public string Name { get; }
    }

    internal class GetCountiesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCountiesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<CountryDto>> Handle(GetCountriesQuery query, CancellationToken cancellationToken)
        {
            return _dbContext.Countries
                .AsNoTracking()
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.Name.Contains(query.Name) && c.Supported)
                .ToListAsync(cancellationToken);
        }
    }
}
