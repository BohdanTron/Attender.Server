using Attender.Server.Application.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable, 
            int pageSize, 
            int pageNumber,
            CancellationToken cancellationToken = new())
            => PaginatedList<TDestination>.Create(queryable, pageSize, pageNumber, cancellationToken);
    }
}
