using Attender.Server.Application.Cities.Queries;
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

        public async Task<List<CountryDto>> Handle(GetCountriesQuery query, CancellationToken cancellationToken)
        {
            var countries = _dbContext.Countries
                .AsNoTracking()
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.Name.Contains(query.Name) && c.Supported)
                .ToListAsync(cancellationToken);

            foreach(var country in await countries)
            {
                country.Cities = await GetPopularCities(country.Id);
            }

            return await countries;
        }

        private async Task<List<CityDto>> GetPopularCities(int countryId)
        {
            var popularCitiesPerCountry = _dbContext.Events
               .Join(_dbContext.Locations, e => e.LocationId, l => l.Id, (e, l) => new { e, l })
               .Join(_dbContext.Cities, cl => cl.l.CityId, c => c.Id, (cl, c) => new { cl, c })
               .Where(a => a.c.CountryId == countryId)
               //.OrderByDescending(z => z.cl.e.Id.ToString().Count())
               .GroupBy(x => new { x.c.Name, x.c.Id, x.c.CountryId })
               .Select(m => new CityDto
               {
                   Name = m.Key.Name,
                   Id = m.Key.Id,
                   CountryId = m.Key.CountryId
               })
               .AsNoTracking()
               .ToListAsync();

            return await popularCitiesPerCountry;
        }
    }
}
