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

namespace Attender.Server.Application.SubCategories.Commands.CreateUserSubCategories
{
    public record CreateUserSubCategoriesCommand : IRequest<Result<int>>
    {
        public int UserId { get; init; }
        public ICollection<CategoryDto> Categories { get; init; } = new List<CategoryDto>();
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

            var subCategories = request.BindAllCategories
                ? await _dbContext.SubCategories.ToListAsync(cancellationToken)
                : await GetSubCategoriesFromCategories(request.Categories, cancellationToken);
            
            var subCategoriesToAdd = subCategories.Except(user.SubCategories).ToList();

            subCategoriesToAdd.ForEach(user.SubCategories.Add);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }

        private async Task<List<SubCategory>> GetSubCategoriesFromCategories(
            IEnumerable<CategoryDto> categories,
            CancellationToken cancellationToken)
        {
            var result = new List<SubCategory>();

            foreach (var category in categories)
            {
                var subCategories = await _dbContext.SubCategories
                    .Where(SubCategoriesPredicate(category))
                    .ToListAsync(cancellationToken);

                result.AddRange(subCategories);
            }

            return result;

            static Expression<Func<SubCategory, bool>> SubCategoriesPredicate(CategoryDto category)
            {
                return category.BindAllSubCategories
                    ? c => c.CategoryId == category.Id
                    : c => category.SubCategoryIds.Contains(c.Id);
            }
        }
    }
}
