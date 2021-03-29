using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Attender.Server.Infrastructure.Auth
{
    public class TokensGenerator : ITokensGenerator
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public TokensGenerator(IAttenderDbContext dbContext, IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateAccessToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey));

            var jwt = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.LifetimeMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return accessToken;
        }

        public async Task<(string access, string refresh)> GenerateAccessRefreshTokens(int userId, string userRole)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey));

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.LifetimeMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var refreshToken = await GenerateRefreshToken(userId, jwt.Id);

            return (accessToken, refreshToken);
        }

        private async Task<string> GenerateRefreshToken(int userId, string accessTokenId)
        {
            var refreshToken = new RefreshToken
            {
                AccessTokenId = accessTokenId,
                Used = false,
                UserId = userId,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(_jwtSettings.RefreshTokenLifetimeYears),
                Value = GenerateRefreshTokenValue()
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return refreshToken.Value;
        }

        private static string GenerateRefreshTokenValue()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
