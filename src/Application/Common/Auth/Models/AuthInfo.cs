using Attender.Server.Application.Common.Auth.Dtos;
using System;

namespace Attender.Server.Application.Common.Auth.Models
{
    public record AuthInfo(AuthTokens Tokens, UserDto? User = null);

    public record AuthTokens(Token AccessToken, Token? RefreshToken = null);

    public record Token(string Value, DateTime Expires);
}
