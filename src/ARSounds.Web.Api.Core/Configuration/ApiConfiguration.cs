namespace ARSounds.Web.Api.Core.Configuration;

public class ApiConfiguration
{
    public string ApiName { get; set; }

    public string VisionApiKey { get; set; }

    public string ApiVersion { get; set; }

    public string Authority { get; set; }

    public string ApiBaseUrl { get; set; }

    public string Audience { get; set; }

    public string OidcSwaggerUIClientId { get; set; }

    public string AdministrationRole { get; set; }

    public bool RequireHttpsMetadata { get; set; }

    public bool CorsAllowAnyOrigin { get; set; }

    public string[] CorsAllowOrigins { get; set; }

    public string[] Scopes { get; set; }
}
