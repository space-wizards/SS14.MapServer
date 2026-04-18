using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SS14.MapServer.Security;

public class ExcludeAnonymousSecurityFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var allowsAnonymousAccess = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>().Any();

        if (allowsAnonymousAccess)
            return;

        operation.Responses?.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses?.Add("403", new OpenApiResponse { Description = "Forbidden" });

        var apiKeyScheme = new OpenApiSecuritySchemeReference(ApiKeyHandler.Name, context.Document);

        operation.Security =
        [
            new OpenApiSecurityRequirement
            {
                [apiKeyScheme] = ["API"]
            }
        ];
    }
}
