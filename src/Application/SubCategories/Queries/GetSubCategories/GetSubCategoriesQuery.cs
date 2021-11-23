using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.SubCategories.Queries.GetSubCategories
{
    public record GetSubCategoriesQuery(int CategoryId, string? Name) : IRequest<List<SubCategoryDto>>;

    internal class GetSubCategoriesQueryHandler : IRequestHandler<GetSubCategoriesQuery, List<SubCategoryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetSubCategoriesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<SubCategoryDto>> Handle(GetSubCategoriesQuery query, CancellationToken cancellationToken)
        {
            return _dbContext.SubCategories
                .Where(s => s.CategoryId == query.CategoryId && (query.Name == null || s.Name.Contains(query.Name)))
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
