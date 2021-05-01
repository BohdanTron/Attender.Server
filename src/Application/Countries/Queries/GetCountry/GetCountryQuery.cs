using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Queries.GetCountry
{
    public class GetCountryQuery : IRequest<List<CountryDto>>
    {
        public GetCountryQuery(string name) => Name = name;

        public string Name { get; }
    }

    internal class GetCountryQueryHandler : IRequestHandler<GetCountryQuery, List<CountryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCountryQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<CountryDto>> Handle(GetCountryQuery query, CancellationToken cancellationToken)
        {
            return _dbContext.Countries
                .AsNoTracking()
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.Name.Contains(query.Name) && c.Supported)
                .ToListAsync(cancellationToken);
        }

    }
}
