using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public GetUserQuery(int id) => Id = id;

        public int Id { get; }
    }

    internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IAttenderDbContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IAttenderDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _context.Users
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        }
    }
}
