using System.Collections.Generic;

namespace Attender.Server.API.Common
{
    public class ErrorResponse
    {
        public ErrorResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public ErrorResponse(string error)
        {
            Errors = new[] { error };
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
