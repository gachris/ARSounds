namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a request to update an existing target.
/// All fields are optional; only the provided fields will be updated.
/// </summary>
public class UpdateTargetRequest
{
    /// <summary>
    /// Gets or sets the updated name of the target.
    /// If provided, this value will replace the target's current name.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target should be trackable.
    /// If provided, this value determines if the target is marked as trackable.
    /// </summary>
    public virtual bool? IsTrackable { get; set; }

    /// <summary>
    /// Gets or sets the updated hexadecimal color code for the target.
    /// If provided, this value will update the target's color.
    /// </summary>
    public virtual string? Color { get; set; }
}
