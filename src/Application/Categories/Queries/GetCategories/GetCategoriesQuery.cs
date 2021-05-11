using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<List<CategoryDto>>
    {
    }

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<CategoryDto>> Handle(GetCategoriesQuery query,CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                    .AsNoTracking()
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                    .Select(i =>i)
                    .ToListAsync(cancellationToken);
        }
    }

}
