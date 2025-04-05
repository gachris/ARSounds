namespace ARSounds.Server.Core.Configuration;

/// <summary>
/// Represents the API configuration settings for the application.
/// </summary>
public class ApiConfiguration
{
    /// <summary>
    /// Gets or sets the name of the API.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the version of the API.
    /// </summary>
    public string Version { get; set; } = null!;

    /// <summary>
    /// Gets or sets the endpoint URL for the Swagger UI.
    /// </summary>
    public string SwaggerEndpoint { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether CORS requests from any origin are allowed.
    /// </summary>
    public bool CorsAllowAnyOrigin { get; set; }

    /// <summary>
    /// Gets or sets the list of allowed origins for CORS.
    /// </summary>
    public string[] CorsAllowOrigins { get; set; } = null!;
}