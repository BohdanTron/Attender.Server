using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Countries.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Queries.GetCountry
{
    public class GetClosestCountries : IRequest<List<CountryDto>>
    {
        public GetClosestCountries(string code) => Code = code;

        public string Code { get; }
    }

    internal class GetClosestCountriesHandler : IRequestHandler<GetClosestCountries, List<CountryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public const int ClosestCountriesCount = 3;

        public GetClosestCountriesHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CountryDto>> Handle(GetClosestCountries query, CancellationToken cancellationToken)
        {
            var currentCountry = await _dbContext.Countries
                 .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(c => c.Code == query.Code, cancellationToken);

            return await GetClosestCountries(currentCountry);
        }

        private async Task<List<CountryDto>> GetClosestCountries(CountryDto countryDto)
        {
            var allCountries = await _dbContext.Countries
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.Supported && c.Code != countryDto.Code)
                .ToListAsync();

            var closestCountries = new List<CountryDto>();

            foreach (var country in allCountries)
            {
                country.Distance = GetDistance(
                                     (double) countryDto.Latitude,
                                     (double) countryDto.Longitude,
                                     (double) country.Latitude,
                                     (double) country.Longitude);

                closestCountries.Add(country);
            }

            return closestCountries.OrderBy(c => c.Distance)
                                         .Take(ClosestCountriesCount)
                                         .ToList();
        }

        private static double GetDistance(
            double currentLatitude,
            double currentLongitude,
            double nextLatitude,
            double nextLongitude,
            char unit = 'K')
        {
            // In case we will get latitude and longitude not from DB
            if (!CoordinateValidator.Validate(currentLatitude, currentLongitude))
                Result.Failure<string>("Invalid origin coordinates supplied.");
            if (!CoordinateValidator.Validate(nextLatitude, nextLongitude))
                Result.Failure<string>("Invalid destination coordinates supplied.");

            const double tolerance = 0.000000001;
            if (Math.Abs(currentLatitude - nextLatitude) < tolerance && Math.Abs(currentLongitude - nextLongitude) < tolerance)
            {
                return 0;
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

            return distance;
        }
    }
}
