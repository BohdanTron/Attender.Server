using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Attender.Server.API.Configuration
{
    public class SwaggerNamespaceSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Title = context.Type.Name;
        }
    }
}
