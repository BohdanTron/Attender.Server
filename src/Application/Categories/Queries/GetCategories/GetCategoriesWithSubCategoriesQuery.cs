using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.SubCategories.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Categories.Queries
{
    public class GetCategoriesWithSubCategoriesQuery : IRequest<List<CategoryDto>>
    {
        [Required]
        public List<int> Ids { get; set; }
    }

    internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesWithSubCategoriesQuery, List<CategoryDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<CategoryDto>> Handle(GetCategoriesWithSubCategoriesQuery query, CancellationToken cancellationToken)
        {
            return  _dbContext.Categories
                    .Include(sc => sc.SubCategories)
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                    .Where(с => query.Ids.Contains(с.Id))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
        }
    }

}
