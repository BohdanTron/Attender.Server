using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
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
        private readonly AuthSettings _authSettings;

        public TokensGenerator(IAttenderDbContext dbContext, IOptions<AuthSettings> authSettings)
        {
            _dbContext = dbContext;
            _authSettings = authSettings.Value;
        }

        public AccessToken GenerateAccessToken()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, _authSettings.Issuer)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.SecurityKey));
            var expires = DateTime.UtcNow.AddMinutes(_authSettings.LifetimeMinutes);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AccessToken(encodedJwt, expires);
        }

        public async Task<(AccessToken access, string refresh)> GenerateAccessRefreshTokens(int userId, string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iss, _authSettings.Issuer),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authSettings.SecurityKey));
            var expires = DateTime.UtcNow.AddMinutes(_authSettings.LifetimeMinutes);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_authSettings.LifetimeMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var accessToken = new AccessToken(encodedJwt, expires);
            var refreshToken = await CreateRefreshToken(userId, jwt.Id);

            return (accessToken, refreshToken);
        }

        private async Task<string> CreateRefreshToken(int userId, string accessTokenId)
        {
            var refreshToken = new RefreshToken
            {
                AccessTokenId = accessTokenId,
                Used = false,
                UserId = userId,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(_authSettings.RefreshTokenLifetimeYears),
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
