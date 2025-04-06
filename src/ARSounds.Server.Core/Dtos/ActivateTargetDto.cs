namespace ARSounds.Server.Core.Dtos;

/// <summary>
/// Represents the data required to activate a target, including a PNG image encoded in base64 format and an optional hexadecimal color code.
/// </summary>
public record ActivateTargetDto
{
    /// <summary>
    /// Gets or sets the PNG image encoded in base64 format.
    /// The image must be in PNG format.
    /// </summary>
    public virtual required string Image { get; set; }

    /// <summary>
    /// Gets or sets an optional hexadecimal color code associated with the target.
    /// </summary>
    public virtual string? Color { get; set; }
}
