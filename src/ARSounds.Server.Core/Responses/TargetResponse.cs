namespace ARSounds.Server.Core.Responses;

/// <summary>
/// Represents a response containing target details.
/// </summary>
public class TargetResponse
{
    /// <summary>
    /// Gets the unique identifier of the target.
    /// </summary>
    public virtual required Guid Id { get; set; }

    /// <summary>
    /// Gets the name of the target.
    /// </summary>
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets the base64-encoded audio data.
    /// </summary>
    public virtual required string Audio { get; set; }

    /// <summary>
    /// Gets a value indicating whether the target is active.
    /// </summary>
    public virtual required bool IsActive { get; set; }

    /// <summary>
    /// Gets the base64-encoded image data.
    /// </summary>
    public virtual string? Image { get; set; }

    /// <summary>
    /// Gets the unique identifier associated with the vision target.
    /// </summary>
    public virtual string? OpenVisionId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the target is trackable.
    /// </summary>
    public virtual bool? IsTrackable { get; set; }

    /// <summary>
    /// Gets the hexadecimal color code associated with the target.
    /// </summary>
    public virtual string? Color { get; set; }

    /// <summary>
    /// Gets the rating of the target.
    /// </summary>
    public virtual int? Rate { get; set; }

    /// <summary>
    /// Gets the creation date and time of the target.
    /// </summary>
    public virtual required DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets the last update date and time of the target.
    /// </summary>
    public virtual required DateTimeOffset Updated { get; set; }
}