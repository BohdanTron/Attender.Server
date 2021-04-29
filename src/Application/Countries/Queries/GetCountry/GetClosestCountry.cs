using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Application.Countries;
using Attender.Server.Application.Countries.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Queries.GetCountry
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

            var listOfClosestCountries = new List<CountryDto>();

            foreach (var country in allCountries)
            {
                country.Distance = GetDistance((double)countryDto.Latitude, (double)countryDto.Longitude, (double)country.Latitude, (double)country.Longitude);

                listOfClosestCountries.Add(country);
            }

            return listOfClosestCountries.OrderBy(d => d.Distance)
                                         .Take(ClosestCountriesCount)
                                         .ToList();
        }

        /// <summary>
        /// Calculate distance between current location and next location
        private static double GetDistance(double currentLocationLatitude, double currentLocationLongitude, double nextLocationLatitude, double nextLocationLongitude, char unit = 'K')
        {
            // In case we will get latitude and longtitude not from DB
            if (!CoordinateValidator.Validate(currentLocationLatitude, currentLocationLongitude))
               Result.Failure<string>("Invalid origin coordinates supplied.");
            if (!CoordinateValidator.Validate(nextLocationLatitude, nextLocationLongitude))
                Result.Failure<string>("Invalid destination coordinates supplied.");

            if (currentLocationLatitude == nextLocationLatitude && currentLocationLongitude == nextLocationLongitude)
            {
                return 0;
            }
            
            var theta = currentLocationLongitude - nextLocationLongitude;

            // Calculate distance between points of two locations in radians
            double distance = Math.Sin(currentLocationLatitude.ToRadian()) * Math.Sin(nextLocationLatitude.ToRadian())
                            + Math.Cos(currentLocationLatitude).ToRadian() * Math.Cos(nextLocationLatitude.ToRadian())
                            * Math.Cos(theta.ToRadian());

            distance = Math.Acos(distance);
            // Convert to degrees
            distance = distance.ToDegrees();
            // 1.1515 is the number of statute miles in a nautical mile
            distance = distance * 60 * 1.1515;

            // 60 * 1.1515 * 1.609344 kilometres to one degree
            if (unit == 'K')
            {
                distance *= 1.609344;
            }
            else if (unit == 'N')
            {
                distance *= 0.8684;
            }
            
            return distance;
        }
    }
}
