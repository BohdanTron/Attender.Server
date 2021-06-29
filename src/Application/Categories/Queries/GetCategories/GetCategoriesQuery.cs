using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery(string LanguageCode) : IRequest<Result<List<CategoryDto>>>;

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
    {
        private const int MaxSubCategoriesCount = 6;

        private readonly IAttenderDbContext _dbContext;

        public GetCategoriesQueryHandler(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            var languageId = await _dbContext.Languages
                .Where(l => l.Code == query.LanguageCode)
                .Select(l => l.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (languageId == default)
                return Result.Failure<List<CategoryDto>>(Errors.Language.CodeNotExist());

            var categories = await _dbContext.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.CategoryDescriptions
                        .Where(d => d.CategoryId == c.Id && d.LanguageId == languageId)
                        .Select(d => d.Text)
                        .First(),
                    SubCategories = c.SubCategories
                        .Select(s => new SubCategoryDto
                        {
                            Id = s.Id,
                            Name = s.Name,
                            CategoryId = s.CategoryId
                        })
                        .Take(MaxSubCategoriesCount)
                })
                .ToListAsync(cancellationToken);

            return Result.Success(categories);
        }
    }

}
