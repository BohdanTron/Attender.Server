using Attender.Server.Application.Common.Interfaces;
using Attender.Server.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Attender.Server.Infrastructure.Auth
{
    //TODO: Consider removing interface and creating static class instead
    public class TokensValidator : ITokensValidator
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

        public async Task<ValidationTokenResult> ValidateRefreshToken(string accessToken, string refreshToken)
        {
            var principal = GetPrincipalFromToken(accessToken, out var errorMessage);
            if (principal is null)
            {
                return new ValidationTokenResult { Errors = new[] { errorMessage } };
            }

            var expiration = long.Parse(principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expirationDate = DateTime.UnixEpoch.AddSeconds(expiration);

            if (expirationDate > DateTime.UtcNow)
            {
                return new ValidationTokenResult { Errors = new[] { "Token hasn't expired yet" } };
            }

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Value == refreshToken);
            if (storedToken is null)
            {
                return new ValidationTokenResult { Errors = new[] { "Refresh token doesn't exist" } };
            }

            if (DateTime.UtcNow > storedToken.ExpiryDate)
            {
                return new ValidationTokenResult { Errors = new[] { "Refresh token has expired" } };
            }

            if (storedToken.Used)
            {
                return new ValidationTokenResult { Errors = new[] { "Refresh token has been used" } };
            }

            var jti = principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti);
            if (jti.Value != storedToken.AccessTokenId)
            {
                return new ValidationTokenResult { Errors = new[] { "Refresh token doesn't match JWT" } };
            }

            return new ValidationTokenResult
            {
                Success = true,
                StoredToken = storedToken
            };
        }

        private ClaimsPrincipal? GetPrincipalFromToken(string token, out string errorMessage)
        {
            errorMessage = string.Empty;
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var parameters = _tokenValidationParameters.Clone();
                parameters.ValidateLifetime = false;

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                return ValidateTokenAlgorithm(validatedToken) ? principal : null;
            }
            catch (SecurityTokenException exception)
            {
                errorMessage = exception.Message;
                return null;
            }
        }

        private static bool ValidateTokenAlgorithm(SecurityToken token)
        {
            return token is JwtSecurityToken jwt &&
                   jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
