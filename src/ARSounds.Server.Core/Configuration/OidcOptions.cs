namespace ARSounds.Server.Core.Configuration;

/// <summary>
/// Provides configuration options for OpenID Connect (OIDC) authentication.
/// </summary>
public class OidcOptions
{
    /// <summary>
    /// Gets or sets the authority URL for the OIDC provider.
    /// </summary>
    public string Authority { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether HTTPS metadata is required.
    /// </summary>
    public bool RequireHttpsMetadata { get; set; }

    /// <summary>
    /// Gets or sets the expected audience for the access tokens.
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// Gets or sets the client ID to be used by Swagger UI for authentication.
    /// </summary>
    public string SwaggerUIClientId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the scopes required for accessing the API.
    /// </summary>
    public string[] Scopes { get; set; } = null!;
}