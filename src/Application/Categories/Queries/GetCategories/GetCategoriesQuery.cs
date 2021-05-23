using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery(IEnumerable<int> Ids) : IRequest<List<CategoryDto>>;

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<CategoryDto>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .AsNoTracking()
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .Where(с => query.Ids.Contains(с.Id))
                .ToListAsync(cancellationToken);
        }
    }

}
