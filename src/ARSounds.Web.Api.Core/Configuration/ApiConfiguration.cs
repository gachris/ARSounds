namespace ARSounds.Web.Api.Core.Configuration;

public record ApiConfiguration(
    string ApiName,
    string VisionApiKey,
    string ApiVersion,
    string Authority,
    string ApiBaseUrl,
    string Audience,
    string OidcSwaggerUIClientId,
    string AdministrationRole,
    bool RequireHttpsMetadata,
    bool CorsAllowAnyOrigin,
    string[] CorsAllowOrigins,
    string[] Scopes);
