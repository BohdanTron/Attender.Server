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
    public class GetSubCategoriesQuery : IRequest<List<SubCategoryDto>>
    {
        public int CategoryId { get; set; }
    }

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
                .AsNoTracking()
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                .Where(c => c.CategoryId == query.CategoryId)
                .ToListAsync(cancellationToken);
        }
    }
}
