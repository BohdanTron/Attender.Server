using System;

namespace Attender.Server.Application.Common.Auth.Models
{
    public record AuthTokens(Token AccessToken, Token? RefreshToken = null);

    public record Token(string Value, DateTime Expires);
}
