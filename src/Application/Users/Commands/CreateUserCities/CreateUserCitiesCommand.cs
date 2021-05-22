using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Commands.CreateUserCities
{
    public class CreateUserCitiesCommand : IRequest<Result<int>>
    {
        public int UserId { get; set; }
        public IEnumerable<int> CityIds { get; set; } = new List<int>();
    }

    public class CreateUserCitiesCommandHandler : IRequestHandler<CreateUserCitiesCommand, Result<int>>
    {
        private readonly IAttenderDbContext _dbContext;

        public CreateUserCitiesCommandHandler(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(CreateUserCitiesCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.Cities)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user.Cities.Any(c => request.CityIds.Contains(c.Id)))
            {
                return Result.Failure<int>(Errors.Cities.AlreadyAppliedForUser());
            }

            var cities = await _dbContext.Cities
                .Where(c => request.CityIds.Contains(c.Id))
                .ToListAsync(cancellationToken);

            cities.ForEach(user.Cities.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}
