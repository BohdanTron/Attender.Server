using Attender.Server.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserDto?>
    {
        public GetUserQuery(string phoneNumber) => PhoneNumber = phoneNumber;

        public string PhoneNumber { get; }
    }

    internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto?>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<UserDto?> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .ProjectTo<UserDto?>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(u => u!.PhoneNumber == query.PhoneNumber, cancellationToken);
        }
    }
}
