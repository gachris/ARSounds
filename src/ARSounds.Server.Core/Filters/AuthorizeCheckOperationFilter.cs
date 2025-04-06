using ARSounds.Server.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ARSounds.Server.Core.Filters;

/// <summary>
/// Operation filter for adding security requirements to Swagger operations based on authorization filters.
/// </summary>
public class AuthorizeCheckOperationFilter : IOperationFilter
{
    #region Fields/Consts

    private readonly SwaggerConfiguration _swaggerConfiguration;

    #endregion

    /// <summary>
    /// Constructor for the AuthorizeCheckOperationFilter class.
    /// </summary>
    /// <param name="swaggerConfiguration">The swagger configuration containing security settings.</param>
    public AuthorizeCheckOperationFilter(SwaggerConfiguration swaggerConfiguration)
    {
        _swaggerConfiguration = swaggerConfiguration;
    }

    #region Methods

    /// <summary>
    /// Applies security requirements to the Swagger operation if any authorization filter is applied.
    /// </summary>
    /// <param name="operation">The Swagger operation being configured.</param>
    /// <param name="context">The context for the Swagger operation filter.</param>
    public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if any filter implements IAuthorizeData
        var hasAuthorize = context.ApiDescription.ActionDescriptor.FilterDescriptors
            .Any(fd => fd.Filter is IAuthorizeData);

        if (hasAuthorize)
        {
            // Add responses for 401 and 403 errors.
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            // Define security requirements for the operation.
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new List<string> { _swaggerConfiguration.Audience }
                    }
                }
            ];
        }
    }

    #endregion
}
