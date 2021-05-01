using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Countries.Helpers;
using Attender.Server.Application.Countries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Countries.Queries.GetClosestCountries
{
    public class GetClosestCountriesQuery : IRequest<List<CountryDto>>
    {
        [Required]
        public string Code { get; set; } = null!;
    }

    internal class GetClosestCountriesHandler : IRequestHandler<GetClosestCountriesQuery, List<CountryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public const int ClosestCountriesCount = 3;

        public GetClosestCountriesHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CountryDto>> Handle(GetClosestCountriesQuery query, CancellationToken cancellationToken)
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
                var result = GetDistance(
                                     (double) countryDto.Latitude,
                                     (double) countryDto.Longitude,
                                     (double) country.Latitude,
                                     (double) country.Longitude);

                if (!result.Succeeded) return Enumerable.Empty<CountryDto>().ToList();

                closestCountries.Add(country);
            }

            return closestCountries.OrderBy(c => c.Distance)
                                         .Take(ClosestCountriesCount)
                                         .ToList();
        }

        private static Result<double> GetDistance(
            double currentLatitude,
            double currentLongitude,
            double nextLatitude,
            double nextLongitude,
            char unit = 'K')
        {
            if (!CoordinateValidator.Validate(currentLatitude, currentLongitude))
                return Result.Failure<double>("Invalid origin coordinates supplied.");
            if (!CoordinateValidator.Validate(nextLatitude, nextLongitude))
                return Result.Failure<double>("Invalid destination coordinates supplied.");

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
