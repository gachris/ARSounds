namespace ARSounds.EntityFramework.Entities;

/// <summary>
/// Represents an audio asset associated with a user.
/// </summary>
public partial class AudioAsset
{
    /// <summary>
    /// Gets or sets the unique identifier for the audio asset.
    /// </summary>
    public virtual required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user associated with the audio asset.
    /// </summary>
    public virtual required string UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the audio asset.
    /// </summary>
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the audio data as a byte array.
    /// </summary>
    public virtual required byte[] Audio { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the audio asset was created.
    /// </summary>
    public virtual required DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the audio asset was last updated.
    /// </summary>
    public virtual required DateTimeOffset Updated { get; set; }

    /// <summary>
    /// Gets or sets the related image asset, if any.
    /// </summary>
    public virtual ImageAsset? ImageAsset { get; set; }
}
