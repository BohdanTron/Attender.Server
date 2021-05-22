using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Attender.Server.API.Configuration
{
    public class DashParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            return value is null
                ? null
                : Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
