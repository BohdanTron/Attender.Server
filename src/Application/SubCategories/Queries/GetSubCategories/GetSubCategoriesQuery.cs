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
    public class GetSubCategoriesQuery : IRequest<IReadOnlyCollection<SubCategoryDto>>
    {
        public int CategoryId { get; set; }
    }

    internal class GetSubCategoriesQueryHandler : IRequestHandler<GetSubCategoriesQuery, IReadOnlyCollection<SubCategoryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetSubCategoriesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<SubCategoryDto>> Handle(GetSubCategoriesQuery query, CancellationToken cancellationToken)
        {
            return await _dbContext.SubCategories
                .AsNoTracking()
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.CategoryId == query.CategoryId)
                .ToListAsync(cancellationToken);
        }
    }
}
