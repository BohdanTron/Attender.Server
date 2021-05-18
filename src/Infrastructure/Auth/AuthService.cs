using Attender.Server.Application.Common.DTOs.Auth;
using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Role = Attender.Server.Domain.Enums.Role;

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

        public async Task<Result<AuthTokens>> Register(UserRegistrationInfoDto dto)
        {
            var exists = await _dbContext.Users
                .AnyAsync(u => u.PhoneNumber == dto.PhoneNumber || u.UserName == dto.UserName ||
                               dto.Email != null && u.Email == dto.Email);

            if (exists)
            {
                return Result.Failure<AuthTokens>(Errors.User.Exists());
            }

            var user = new User
            {
                PhoneNumber = dto.PhoneNumber,
                UserName = dto.UserName,
                Email = dto.Email,
                RoleId = (byte) Role.User,
                AvatarId = dto.AvatarId
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

        public async Task<Result<AuthTokens>> RefreshToken(RefreshTokenDto dto)
        {
            var (accessToken, refreshToken) = (dto.AccessToken, dto.RefreshToken);

            var validation = await _tokensValidator.ValidateRefreshToken(accessToken, refreshToken);
            if (!validation.Succeeded)
            {
                return Result.Failure<AuthTokens>(validation.Error!);
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
