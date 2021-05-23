using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public record GetUserQuery(string? UserName) : IRequest<Result<UserDto>>;

    internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IAttenderDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(u => u.UserName == query.UserName, cancellationToken);

            return user is null
                ? Result.Failure<UserDto>("user_not_found", "User doesn't exist")
                : Result.Success(user);
        }
    }
}
