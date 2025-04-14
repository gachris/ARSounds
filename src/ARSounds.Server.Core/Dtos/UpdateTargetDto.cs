namespace ARSounds.Server.Core.Dtos;

/// <summary>
/// Represents the data used to update a target, with each field being optional.
/// </summary>
public record UpdateTargetDto
{
    /// <summary>
    /// Gets or sets the updated name of the target.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target should be trackable.
    /// </summary>
    public virtual bool? IsTrackable { get; set; }

    /// <summary>
    /// Gets or sets the updated hexadecimal color code for the target.
    /// </summary>
    public virtual string? Color { get; set; }
}
