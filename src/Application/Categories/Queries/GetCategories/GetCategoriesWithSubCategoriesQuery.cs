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
        public int Id { get; set; }
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

        public Task<List<CategoryDto>> Handle(GetCategoriesWithSubCategoriesQuery query,CancellationToken cancellationToken)
        {
            var subCategories = _dbContext.SubCategories
                    .AsNoTracking()
                    .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                    .Where(c => c.CategoryId == query.Id)
                    .ToListAsync(cancellationToken);

            var categories = _dbContext.Categories
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                   .Where(a => a.Id == query.Id)
                    .Select(i => new CategoryDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        SubCategories = subCategories.Result
                    })
                     .AsNoTracking()
                    .ToListAsync(cancellationToken);

            return categories;
        }
    }

}
