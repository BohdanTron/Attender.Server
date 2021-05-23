using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Countries.Services;
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
    public record GetClosestCountriesQuery(string? Code) : IRequest<List<CountryDto>>;

    internal class GetClosestCountriesHandler : IRequestHandler<GetClosestCountriesQuery, List<CountryDto>>
    {
        public const int MaxCountriesCount = 3;

        private readonly IAttenderDbContext _dbContext;
        private readonly PopularCitiesService _popularCitiesService;

        public GetClosestCountriesHandler(
            IAttenderDbContext dbContext,
            PopularCitiesService popularCitiesService)
        {
            _dbContext = dbContext;
            _popularCitiesService = popularCitiesService;
        }

        public async Task<List<CountryDto>> Handle(GetClosestCountriesQuery query, CancellationToken cancellationToken)
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

        private async Task<List<CountryDto>> GetClosestCountriesTo(Country currentCountry, CancellationToken cancellationToken)
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
            var cities = _popularCitiesService.Get(countryIds);

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
    }
}
