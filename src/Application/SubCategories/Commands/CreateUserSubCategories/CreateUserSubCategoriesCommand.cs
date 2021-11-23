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
    public record CreateUserSubCategoriesCommand : IRequest<Result<int>>
    {
        public int UserId { get; init; }
        public ICollection<int> SubCategoryIds { get; init; } = new List<int>();
        public bool BindAllCategories { get; init; }
    }

    internal class CreateUserSubCategoriesCommandHandler : IRequestHandler<CreateUserSubCategoriesCommand, Result<int>>
    {
        private readonly IAttenderDbContext _dbContext;

        public CreateUserSubCategoriesCommandHandler(IAttenderDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Result<int>> Handle(
            CreateUserSubCategoriesCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.SubCategories)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user is null)
                return Result.Failure<int>(Errors.User.NotExist());

            var subCategories = await _dbContext.SubCategories
                .Where(s => request.BindAllCategories || request.SubCategoryIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            var subCategoriesToAdd = subCategories.Except(user.SubCategories).ToList();

            subCategoriesToAdd.ForEach(user.SubCategories.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}
