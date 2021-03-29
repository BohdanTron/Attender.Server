﻿using Attender.Server.Application.Common.Interfaces;
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

        public async Task<AuthResult> Register(string phoneNumber, string userName, string? email)
        {
            var exists = await _dbContext.Users.AnyAsync(u =>
                u.PhoneNumber == phoneNumber || u.UserName == userName || email != null && u.Email == email);

            if (exists)
            {
                return new AuthResult
                {
                    Success = false,
                    Errors = new[] { "User already exist" }
                };
            }

            var role = await _dbContext.Roles.FindAsync((byte) Roles.User);
            var user = new User
            {
                PhoneNumber = phoneNumber,
                UserName = userName,
                Email = email,
                Role = role
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return await Login(user.Id, user.Role.Name);
        }

        public async Task<AuthResult> LoginOrGenerateAccessToken(string phoneNumber)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user is not null) return await Login(user.Id, user.Role!.Name);

            var accessToken = _tokensGenerator.GenerateAccessToken();
            return new AuthResult
            {
                Success = true,
                AccessToken = accessToken
            };
        }

        public async Task<AuthResult> RefreshToken(string accessToken, string refreshToken)
        {
            var validation = await _tokensValidator.ValidateRefreshToken(accessToken, refreshToken);
            if (!validation.Success)
            {
                return new AuthResult { Errors = validation.Errors };
            }

            var storedToken = validation.StoredToken;

            storedToken.Used = true;
            _dbContext.RefreshTokens.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == storedToken.UserId);

            return await Login(user.Id, user.Role!.Name);
        }

        private async Task<AuthResult> Login(int userId, string userRole)
        {
            var (access, refresh) = await _tokensGenerator.GenerateAccessRefreshTokens(userId, userRole);

            return new AuthResult
            {
                Success = true,
                AccessToken = access,
                RefreshToken = refresh
            };
        }
    }
}