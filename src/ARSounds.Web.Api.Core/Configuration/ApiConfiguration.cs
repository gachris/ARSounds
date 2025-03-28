namespace ARSounds.Web.Api.Core.Configuration;

public class ApiConfiguration
{
    public string ApiName { get; set; } = null!;

    public string VisionApiKey { get; set; } = null!;

    public string ApiVersion { get; set; } = null!;

    public string Authority { get; set; } = null!;

    public string ApiBaseUrl { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public string OidcSwaggerUIClientId { get; set; } = null!;

    public string AdministrationRole { get; set; } = null!;

    public bool RequireHttpsMetadata { get; set; } 

    public bool CorsAllowAnyOrigin { get; set; } 

    public string[] CorsOrigins { get; set; } = null!;

    public string[] Scopes { get; set; } = null!;
}
