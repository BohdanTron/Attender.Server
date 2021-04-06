using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Domain.Entities;
using Attender.Server.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Attender.Server.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly ITokensGenerator _tokensGenerator;
        private readonly ITokensValidator _tokensValidator;

        public AuthService(
            IAttenderDbContext dbContext,
            ITokensGenerator tokensGenerator,
            ITokensValidator tokensValidator)
        {
            _dbContext = dbContext;
            _tokensGenerator = tokensGenerator;
            _tokensValidator = tokensValidator;
        }

        public async Task<Result<AuthTokens>> Register(string phoneNumber, string userName, string? email)
        {
            var exists = await _dbContext.Users.AnyAsync(u =>
                u.PhoneNumber == phoneNumber || u.UserName == userName || email != null && u.Email == email);

            if (exists)
            {
                return Result.Failure<AuthTokens>("User with such settings already exists");
            }

            var user = new User
            {
                PhoneNumber = phoneNumber,
                UserName = userName,
                Email = email,
                RoleId = (byte) Roles.User
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var tokens = await _tokensGenerator.GenerateAuthTokens(user.Id, user.UserName);
            return Result.Success(tokens);
        }

        public async Task<AuthTokens> LoginOrGenerateAccessToken(string phoneNumber)
        {
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user is not null)
            {
                return await _tokensGenerator.GenerateAuthTokens(user.Id, user.UserName);
            }

            var accessToken = _tokensGenerator.GenerateAccessToken();
            return new AuthTokens(accessToken);
        }

        public async Task<Result<AuthTokens>> RefreshToken(string accessToken, string refreshToken)
        {
            var validation = await _tokensValidator.ValidateRefreshToken(accessToken, refreshToken);
            if (!validation.Succeeded)
            {
                return Result.Failure<AuthTokens>(validation.Errors);
            }

            var storedToken = validation.Data;

            storedToken!.Used = true;
            _dbContext.RefreshTokens.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            var user = await _dbContext.Users
                .SingleAsync(u => u.Id == storedToken.UserId);

            var tokens = await _tokensGenerator.GenerateAuthTokens(user.Id, user.UserName);
            return Result.Success(tokens);
        }
    }
}
