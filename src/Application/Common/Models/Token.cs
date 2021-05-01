using System;

namespace Attender.Server.Application.Common.Models
{
    public record Token (string Value, DateTime Expires);
}
