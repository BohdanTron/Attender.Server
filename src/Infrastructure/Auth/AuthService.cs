using Attender.Server.Application.Common.Auth.Dtos;
using Attender.Server.Application.Common.Auth.Models;
using Attender.Server.Application.Common.Auth.Services;
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
        private readonly TokensGenerator _tokensGenerator;
        private readonly TokensValidator _tokensValidator;

        public AuthService(
            IAttenderDbContext dbContext,
            TokensGenerator tokensGenerator,
            TokensValidator tokensValidator)
        {
            _dbContext = dbContext;
            _tokensGenerator = tokensGenerator;
            _tokensValidator = tokensValidator;
        }

        public async Task<Result<AuthInfo>> RegisterUser(UserRegistrationInfoDto dto)
        {
            var exists = await _dbContext.Users
                .AnyAsync(u => u.PhoneNumber == dto.PhoneNumber || u.UserName == dto.UserName ||
                               dto.Email != null && u.Email == dto.Email);

            if (exists) 
                return Result.Failure<AuthInfo>(Errors.User.Exists());

            var language = await _dbContext.Languages.FirstOrDefaultAsync(l => l.Code == dto.LanguageCode);
            if (language is null)
                return Result.Failure<AuthInfo>(Errors.Language.CodeNotExist());

            var user = new User
            {
                PhoneNumber = dto.PhoneNumber,
                UserName = dto.UserName,
                Email = dto.Email,
                RoleId = (byte) Role.User,
                AvatarId = dto.AvatarId,
                Language = language
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var tokens = await _tokensGenerator.GenerateAuthTokens(user.Id, user.UserName);
            var result = new AuthInfo(tokens, ToUserDto(user));

            return Result.Success(result);
        }

        public async Task<AuthInfo> LoginOrGenerateAccessToken(string phoneNumber)
        {
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user is not null)
            {
                var tokens = await _tokensGenerator.GenerateAuthTokens(user.Id, user.UserName);
                return new AuthInfo(tokens, ToUserDto(user));
            }

            var accessToken = new AuthTokens(_tokensGenerator.GenerateAccessToken());
            return new AuthInfo(accessToken);
        }

        public async Task<Result<AuthTokens>> RefreshToken(RefreshTokenDto dto)
        {
            var (accessToken, refreshToken) = dto;

            var validation = await _tokensValidator.ValidateRefreshToken(accessToken, refreshToken);
            if (!validation.Succeeded)
                return Result.Failure<AuthTokens>(validation.Error!);

            var storedToken = validation.Data;

            storedToken!.Used = true;
            _dbContext.RefreshTokens.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            var user = await _dbContext.Users
                .SingleAsync(u => u.Id == storedToken.UserId);

            var tokens = await _tokensGenerator.GenerateAuthTokens(user.Id, user.UserName);
            return Result.Success(tokens);
        }

        private static UserDto ToUserDto(User user)
        {
            return new()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
