using System.Collections.Generic;

namespace Attender.Server.Application.Common.Models
{
    public class AuthResult
    {
        public AccessToken? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public bool Success { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();
    }
}
