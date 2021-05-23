using Attender.Server.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private const int MaxSubCategoriesCount = 6;

        private readonly IAttenderDbContext _dbContext;

        public GetCategoriesQueryHandler(IAttenderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<CategoryDto>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
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
        }
    }

}
