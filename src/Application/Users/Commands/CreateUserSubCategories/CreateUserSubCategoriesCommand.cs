using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Commands.CreateUserSubCategories
{
    public record CreateUserSubCategoriesCommand(int UserId, IEnumerable<int> SubCategoryIds) : IRequest<Result<int>>;

    internal class CreateUserSubCategoriesCommandHandler : IRequestHandler<CreateUserSubCategoriesCommand, Result<int>>
    {
        private readonly IAttenderDbContext _dbContext;

        public CreateUserSubCategoriesCommandHandler(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<int>> Handle(CreateUserSubCategoriesCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.SubCategories)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user.SubCategories.Any(s => request.SubCategoryIds.Contains(s.Id)))
            {
                return Result.Failure<int>(Errors.SubCategories.AlreadyAppliedForUser());
            }

            var subCategories = await _dbContext.SubCategories
                .Where(s => request.SubCategoryIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            subCategories.ForEach(user.SubCategories.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}
