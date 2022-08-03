using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Server.Api.Schema;

/// <summary>
/// A schema filter that marks all non nullable properties as required.
/// </summary>
public class RequireNonNullablePropertiesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        model.Properties
            .Where(x => !x.Value.Nullable && !model.Required.Contains(x.Key))
            .Select(x => x.Key)
            .ToList()
            .ForEach(key => model.Required.Add(key));
    }
}