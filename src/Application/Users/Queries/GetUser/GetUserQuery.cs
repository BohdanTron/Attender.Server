﻿using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Attender.Server.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<Result<UserDto>>
    {
        [Required]
        public string PhoneNumber { get; set; } = null!;
    }

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
                .ProjectTo<UserDto?>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(u => u!.PhoneNumber == query.PhoneNumber, cancellationToken);

            return user is null
                ? Result.Failure<UserDto>("User doesn't exist")
                : Result.Success(user);
        }
    }
}
