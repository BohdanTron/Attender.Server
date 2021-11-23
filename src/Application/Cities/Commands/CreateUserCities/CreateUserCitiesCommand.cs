using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Cities.Commands.CreateUserCities
{
    public record CreateUserCitiesCommand : IRequest<Result<int>>
    {
        public int UserId { get; init; }
        public ICollection<CountryDto> Countries { get; init; } = new List<CountryDto>();
        public bool BindAllCountries { get; init; }
    }

    public class CreateUserCitiesCommandHandler : IRequestHandler<CreateUserCitiesCommand, Result<int>>
    {
        private readonly IAttenderDbContext _dbContext;

        public CreateUserCitiesCommandHandler(IAttenderDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Result<int>> Handle(CreateUserCitiesCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.Cities)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
                return Result.Failure<int>(Errors.User.NotExist());

            var cities = request.BindAllCountries
                ? await GetAllSupportedCities(cancellationToken)
                : await GetCitiesFromCountries(request.Countries, cancellationToken);

            var citiesToAdd = cities.Except(user.Cities).ToList();

            citiesToAdd.ForEach(user.Cities.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }

        private Task<List<City>> GetAllSupportedCities(CancellationToken cancellationToken)
        {
            return _dbContext.Countries
                .Where(c => c.Supported)
                .SelectMany(c => c.Cities)
                .ToListAsync(cancellationToken);
        }

        private async Task<List<City>> GetCitiesFromCountries(IEnumerable<CountryDto> countries, CancellationToken cancellationToken)
        {
            var result = new List<City>();

            foreach (var country in countries)
            {
                var cities = await _dbContext.Cities
                    .Where(CitiesPredicate(country))
                    .ToListAsync(cancellationToken);

                result.AddRange(cities);
            }

            return result;

            static Expression<Func<City, bool>> CitiesPredicate(CountryDto country)
            {
                return country.BindAllCities
                    ? c => c.Country!.Supported && c.CountryId == country.Id
                    : c => c.Country!.Supported && c.CountryId == country.Id && country.CityIds.Contains(c.Id);
            }
        }
    }
}
