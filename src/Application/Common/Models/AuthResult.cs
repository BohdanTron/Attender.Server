using System.Collections.Generic;

namespace Attender.Server.Application.Common.Models
{
    public class AuthResult
    {
        public Token? AccessToken { get; set; }
        public Token? RefreshToken { get; set; }
        public bool Success { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();
    }
}
