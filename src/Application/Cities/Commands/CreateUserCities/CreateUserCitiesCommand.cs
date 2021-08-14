using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Cities.Commands.CreateUserCities
{
    public record CreateUserCitiesCommand(int UserId, ICollection<int> CityIds) : IRequest<Result<int>>;

    public class CreateUserCitiesCommandHandler : IRequestHandler<CreateUserCitiesCommand, Result<int>>
    {
        private readonly IAttenderDbContext _dbContext;

        public CreateUserCitiesCommandHandler(IAttenderDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Result<int>> Handle(CreateUserCitiesCommand request, CancellationToken cancellationToken)
        {
            var (userId, cityIds) = request;

            var user = await _dbContext.Users
                .Include(u => u.Cities)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user.Cities.Any(c => cityIds.Contains(c.Id)))
                return Result.Failure<int>(Errors.Cities.AlreadyAppliedForUser());

            var cities = await _dbContext.Cities
                .Where(c => cityIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            cities.ForEach(user.Cities.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}
