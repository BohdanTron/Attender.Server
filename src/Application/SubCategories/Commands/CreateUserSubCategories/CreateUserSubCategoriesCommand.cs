using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.SubCategories.Commands.CreateUserSubCategories
{
    public record CreateUserSubCategoriesCommand(int UserId, ICollection<int> SubCategoryIds) : IRequest<Result<int>>;

    internal class CreateUserSubCategoriesCommandHandler : IRequestHandler<CreateUserSubCategoriesCommand, Result<int>>
    {
        private readonly IAttenderDbContext _dbContext;

        public CreateUserSubCategoriesCommandHandler(IAttenderDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Result<int>> Handle(
            CreateUserSubCategoriesCommand request,
            CancellationToken cancellationToken)
        {
            var (userId, subCategoryIds) = request;

            var user = await _dbContext.Users
                .Include(u => u.SubCategories)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user.SubCategories.Any(s => subCategoryIds.Contains(s.Id)))
                return Result.Failure<int>(Errors.SubCategories.AlreadyAppliedForUser());

            var subCategories = await _dbContext.SubCategories
                .Where(s => subCategoryIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            subCategories.ForEach(user.SubCategories.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}
