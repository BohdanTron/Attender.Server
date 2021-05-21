using Attender.Server.Application.Cities.Queries;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Countries.Dtos;
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
    public class GetCountriesQuery : IRequest<IReadOnlyCollection<CountryDto>>
    {
        public string? Name { get; set; }
    }

    internal class GetCountiesQueryHandler : IRequestHandler<GetCountriesQuery, IReadOnlyCollection<CountryDto>>
    {
        private const int MaxCitiesCount = 5;

        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCountiesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<CountryDto>> Handle(GetCountriesQuery query, CancellationToken cancellationToken)
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

            var cities = GetPopularCitiesLookup(countryIds);

            countries.ForEach(country => country.Cities = cities[country.Id]);

            return countries;
        }

        private ILookup<int, CityDto> GetPopularCitiesLookup(IEnumerable<int> countryIds)
        {
            var cities = _dbContext.Events
                .Join(_dbContext.Locations, e => e.LocationId, l => l.Id, (e, l) => new { e, l })
                .Join(_dbContext.Cities, el => el.l.CityId, c => c.Id, (el, c) => new { el, c })
                .Where(elc => countryIds.Contains(elc.c.CountryId))
                .GroupBy(elc => new { elc.c.Name, elc.c.Id, elc.c.CountryId })
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Key.CountryId)
                .Select(group => new CityDto
                {
                    Id = group.Key.Id,
                    Name = group.Key.Name,
                    CountryId = group.Key.CountryId
                })
                .Take(MaxCitiesCount)
                .AsNoTracking()
                .ToLookup(c => c.CountryId);

            return cities;
        }
    }
}
