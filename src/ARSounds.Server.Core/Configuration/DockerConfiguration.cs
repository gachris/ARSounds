namespace ARSounds.Server.Core.Configuration;

/// <summary>
/// Provides configuration settings specific to Docker operations.
/// </summary>
public class DockerConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether the CA certificate should be updated.
    /// </summary>
    public virtual bool UpdateCaCertificate { get; set; }
}
