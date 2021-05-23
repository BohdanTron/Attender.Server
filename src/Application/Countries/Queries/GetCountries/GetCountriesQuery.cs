using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Countries.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Queries.GetCountries
{
    public record GetCountriesQuery(string? Name) : IRequest<List<CountryDto>>;

    internal class GetCountiesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly PopularCitiesService _popularCitiesService;

        public GetCountiesQueryHandler(
            IAttenderDbContext dbContext,
            IMapper mapper,
            PopularCitiesService popularCitiesService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _popularCitiesService = popularCitiesService;
        }

        public async Task<List<CountryDto>> Handle(GetCountriesQuery query, CancellationToken cancellationToken)
        {
            if (query.Name is null)
                return Enumerable.Empty<CountryDto>().ToList();

            var countries = await _dbContext.Countries
                .Where(c => c.Name.Contains(query.Name))
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .OrderBy(c => c.Id)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var countryIds = countries.Select(c => c.Id);

            var cities = _popularCitiesService.Get(countryIds);

            countries.ForEach(country => country.Cities = cities[country.Id]);

            return countries;
        }
    }
}
