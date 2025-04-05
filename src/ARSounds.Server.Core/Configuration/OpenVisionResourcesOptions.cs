namespace ARSounds.Server.Core.Configuration;

/// <summary>
/// Provides configuration options for connecting to the OpenVision service.
/// </summary>
public class OpenVisionResourcesOptions
{
    /// <summary>
    /// Gets or sets the name of the application using OpenVision.
    /// </summary>
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the base URL of the OpenVision server.
    /// </summary>
    public string ServerUrl { get; set; } = null!;

    /// <summary>
    /// Gets or sets the API key for accessing the OpenVision database.
    /// </summary>
    public string DatabaseApiKey { get; set; } = null!;
}
