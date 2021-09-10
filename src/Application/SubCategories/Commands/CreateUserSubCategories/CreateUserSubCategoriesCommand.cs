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

            if (user is null)
                return Result.Failure<int>(Errors.User.NotExist());

            var subCategories = await _dbContext.SubCategories
                .Where(s => subCategoryIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            var subCategoriesToAdd = subCategories.Except(user.SubCategories).ToList();

            subCategoriesToAdd.ForEach(user.SubCategories.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}
