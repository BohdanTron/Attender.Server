using Attender.Server.Domain.Entities;
using System.Collections.Generic;

namespace Attender.Server.Application.Common.Models
{
    public class ValidationTokenResult
    {
        public RefreshToken StoredToken { get; set; } = default!;
        public bool Success { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();
    }
}
