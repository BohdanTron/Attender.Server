using Attender.Server.Application.Cities.Queries;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Countries.DTOs;
using Attender.Server.Application.Countries.Helpers;
using Attender.Server.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Queries.GetClosestCountries
{
    public class GetClosestCountriesQuery : IRequest<IReadOnlyCollection<CountryDto>>
    {
        public string? Code { get; set; }
    }

    internal class GetClosestCountriesHandler : IRequestHandler<GetClosestCountriesQuery, IReadOnlyCollection<CountryDto>>
    {
        private const int MaxCitiesCount = 5;
        public const int MaxCountriesCount = 3;

        private readonly IAttenderDbContext _dbContext;

        public GetClosestCountriesHandler(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<CountryDto>> Handle(GetClosestCountriesQuery query, CancellationToken cancellationToken)
        {
            if (query.Code is null)
                return Enumerable.Empty<CountryDto>().ToList();

            var currentCountry = await _dbContext.Countries
                .FirstOrDefaultAsync(c => c.Code == query.Code && c.Supported &&
                                          c.Longitude != null && c.Latitude != null, cancellationToken);

            if (currentCountry is null)
                return Enumerable.Empty<CountryDto>().ToList();

            return await GetClosestCountriesTo(currentCountry, cancellationToken);
        }

        private async Task<IReadOnlyCollection<CountryDto>> GetClosestCountriesTo(Country currentCountry, CancellationToken cancellationToken)
        {
            var allCountries = await _dbContext.Countries
                .Where(c => c.Supported && c.Code != currentCountry.Code && c.Longitude != null && c.Latitude != null)
                .ToListAsync(cancellationToken);

            var closestCountries = new List<(Country Country, double Distance)>();

            foreach (var country in allCountries)
            {
                // TODO: Make Longitude and Latitude columns not nullable in db
                if (currentCountry.Latitude is null || currentCountry.Longitude is null ||
                    country.Latitude is null || country.Longitude is null)
                    continue;

                var result = GetDistance(
                                     (double) currentCountry.Latitude,
                                     (double) currentCountry.Longitude,
                                     (double) country.Latitude,
                                     (double) country.Longitude);

                if (!result.Succeeded)
                    return Enumerable.Empty<CountryDto>().ToList();

                var distance = result.Data;

                closestCountries.Add((country, distance));
            }

            var countryIds = closestCountries.Select(c => c.Country.Id);
            var cities = GetPopularCitiesLookup(countryIds);

            return closestCountries
                .OrderBy(c => c.Distance)
                .Take(MaxCountriesCount)
                .Select(c => new CountryDto
                {
                    Id = c.Country.Id,
                    Code = c.Country.Code,
                    Name = c.Country.Name,
                    Cities = cities[c.Country.Id]
                })
                .ToList();
        }

        private static Result<double> GetDistance(
            double currentLatitude,
            double currentLongitude,
            double nextLatitude,
            double nextLongitude,
            char unit = 'K')
        {
            // TODO: Move errors to constants
            if (!CoordinateValidator.Validate(currentLatitude, currentLongitude))
                return Result.Failure<double>("invalid_coordinates", "Invalid origin coordinates supplied");
            if (!CoordinateValidator.Validate(nextLatitude, nextLongitude))
                return Result.Failure<double>("invalid_coordinates", "Invalid destination coordinates supplied.");

            const double tolerance = 0.000000001;
            if (Math.Abs(currentLatitude - nextLatitude) < tolerance && Math.Abs(currentLongitude - nextLongitude) < tolerance)
            {
                return Result.Success(0.0);
            }

            var theta = currentLongitude - nextLongitude;

            // Calculate distance between points of two locations in radians
            var distance = Math.Sin(currentLatitude.ToRadian()) * Math.Sin(nextLatitude.ToRadian())
                            + Math.Cos(currentLatitude).ToRadian() * Math.Cos(nextLatitude.ToRadian())
                            * Math.Cos(theta.ToRadian());

            distance = Math.Acos(distance);

            // Convert to degrees
            distance = distance.ToDegrees();

            // 1.1515 is the number of statute miles in a nautical mile
            distance = distance * 60 * 1.1515;

            switch (unit)
            {
                // 60 * 1.1515 * 1.609344 kilometers to one degree
                case 'K':
                    distance *= 1.609344;
                    break;
                case 'N':
                    distance *= 0.8684;
                    break;
            }

            return Result.Success(distance);
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
