namespace ARSounds.Server.Core.Dtos;

/// <summary>
/// Represents the data required to create a new target.
/// </summary>
public record CreateTargetDto
{
    /// <summary>
    /// Gets or sets the name of the target.
    /// </summary>
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the audio data encoded in base64 format.
    /// </summary>
    public virtual required string Audio { get; set; }
}
