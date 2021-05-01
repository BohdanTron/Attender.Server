using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Countries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Queries.GetCountries
{
    public class GetCountriesQuery : IRequest<List<CountryDto>>
    {
        [Required]
        public string Name { get; set; } = null!;
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
