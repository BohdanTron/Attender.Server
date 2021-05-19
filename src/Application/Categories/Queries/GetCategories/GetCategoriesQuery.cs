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
    public class GetCategoriesQuery : IRequest<IReadOnlyCollection<CategoryDto>>
    {
        public IEnumerable<int> Ids { get; set; } = new List<int>();
    }

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyCollection<CategoryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<CategoryDto>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .Where(с => query.Ids.Contains(с.Id))
                .ToListAsync(cancellationToken);
        }
    }

}
