using Attender.Server.Application.Common.Auth.Models;
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
    public class TokensGenerator
    {
        private readonly IAttenderDbContext _dbContext;
        private readonly AuthOptions _authOptions;

        public TokensGenerator(IAttenderDbContext dbContext, IOptions<AuthOptions> authOptions)
        {
            _dbContext = dbContext;
            _authOptions = authOptions.Value;
        }

        public Token GenerateAccessToken()
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Iss, _authOptions.Issuer)
            };

            return GenerateAccessToken(claims);
        }

        public async Task<AuthTokens> GenerateAuthTokens(int userId, string userName)
        {
            var jwtId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Iss, _authOptions.Issuer),
                new(JwtRegisteredClaimNames.Jti, jwtId),
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, userName),
            };

            var accessToken = GenerateAccessToken(claims);
            var refreshToken = await CreateRefreshToken(userId, jwtId);

            return new AuthTokens(accessToken, refreshToken);
        }

        private Token GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authOptions.SecurityKey));
            var expires = DateTime.UtcNow.AddMinutes(_authOptions.LifetimeMinutes);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new Token(encodedJwt, expires);
        }

        private async Task<Token> CreateRefreshToken(int userId, string accessTokenId)
        {
            //var expires = _authOptions.RefreshTokenLifetimeYears;

            // Added for test purpose, will be replaces with a real value in the future
            var expires = DateTime.UtcNow.AddMinutes(5);

            var refreshToken = new RefreshToken
            {
                AccessTokenId = accessTokenId,
                Used = false,
                UserId = userId,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = expires, //DateTime.UtcNow.AddYears(expires),
                Value = GenerateRefreshTokenValue()
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            var result = new Token(refreshToken.Value, refreshToken.ExpiryDate);
            return result;
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
