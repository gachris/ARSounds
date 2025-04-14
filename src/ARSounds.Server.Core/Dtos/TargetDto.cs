namespace ARSounds.Server.Core.Dtos;

/// <summary>
/// Represents a data transfer object for a target.
/// </summary>
public record TargetDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the target.
    /// </summary>
    public virtual required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the target.
    /// </summary>
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the base64-encoded audio data.
    /// </summary>
    public virtual required string Audio { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target is active.
    /// </summary>
    public virtual required bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the base64-encoded image data.
    /// </summary>
    public virtual string? Image { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier associated with the vision target.
    /// </summary>
    public virtual string? OpenVisionId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target is trackable.
    /// </summary>
    public virtual bool? IsTrackable { get; set; }

    /// <summary>
    /// Gets or sets the hexadecimal color code associated with the target.
    /// </summary>
    public virtual string? Color { get; set; }

    /// <summary>
    /// Gets or sets the rating of the target.
    /// </summary>
    public virtual int? Rate { get; set; }

    /// <summary>
    /// Gets or sets the creation date and time of the target.
    /// </summary>
    public virtual required DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets the last update date and time of the target.
    /// </summary>
    public virtual required DateTimeOffset Updated { get; set; }
}
