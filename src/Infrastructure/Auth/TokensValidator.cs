using Attender.Server.Application.Common.Helpers;
using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Attender.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Attender.Server.Infrastructure.Auth
{
    public class TokensValidator
    {
        private readonly IAttenderDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokensValidator(
            IAttenderDbContext context,
            TokenValidationParameters tokenValidationParameters)
        {
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<Result<RefreshToken>> ValidateRefreshToken(string accessToken, string refreshToken)
        {
            var result = GetPrincipalFromToken(accessToken);
            if (!result.Succeeded)
            {
                return Result.Failure<RefreshToken>(result.Error!);
            }

            var principal = result.Data;

            var expiration = long.Parse(principal!.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expirationDate = DateTime.UnixEpoch.AddSeconds(expiration);
            if (expirationDate > DateTime.UtcNow)
            {
                return Result.Failure<RefreshToken>(Errors.Auth.AccessTokenNotExpired());
            }

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Value == refreshToken);
            if (storedToken is null)
            {
                return Result.Failure<RefreshToken>(Errors.Auth.RefreshTokenNotExist());
            }

            if (DateTime.UtcNow > storedToken.ExpiryDate)
            {
                return Result.Failure<RefreshToken>(Errors.Auth.RefreshTokenExpired());
            }

            if (storedToken.Used)
            {
                return Result.Failure<RefreshToken>(Errors.Auth.RefreshTokenUsed());
            }

            var jti = principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti);
            if (jti.Value != storedToken.AccessTokenId)
            {
                return Result.Failure<RefreshToken>(Errors.Auth.RefreshTokenNotMatchJwt());
            }

            return Result.Success(storedToken);
        }

        private Result<ClaimsPrincipal> GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var parameters = _tokenValidationParameters.Clone();
                parameters.ValidateLifetime = false;

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                return ValidateTokenAlgorithm(validatedToken)
                    ? Result.Success(principal)
                    : Result.Failure<ClaimsPrincipal>(Errors.Auth.AccessTokenInvalidAlgorithm());
            }
            catch (SecurityTokenException e)
            {
                return Result.Failure<ClaimsPrincipal>(Errors.Auth.AccessTokenSecurityIssue(e.Message));
            }
            catch (ArgumentException)
            {
                return Result.Failure<ClaimsPrincipal>(Errors.Auth.AccessTokenInvalid());
            }
        }

        private static bool ValidateTokenAlgorithm(SecurityToken token)
        {
            return token is JwtSecurityToken jwt &&
                   jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
