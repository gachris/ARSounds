namespace ARSounds.EntityFramework.Entities;

/// <summary>
/// Represents an image asset that is associated with an audio asset.
/// </summary>
public class ImageAsset
{
    /// <summary>
    /// Gets or sets the unique identifier for the image asset.
    /// </summary>
    public virtual required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated audio asset.
    /// </summary>
    public virtual required Guid AudioAssetId { get; set; }

    /// <summary>
    /// Gets or sets the OpenVision identifier for the image asset.
    /// </summary>
    public virtual required string OpenVisionId { get; set; }

    /// <summary>
    /// Gets or sets the image data as a byte array.
    /// </summary>
    public virtual required byte[] Image { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the image asset is trackable.
    /// </summary>
    public virtual required bool IsTrackable { get; set; }

    /// <summary>
    /// Gets or sets the color information associated with the image asset.
    /// </summary>
    public virtual required string Color { get; set; }

    /// <summary>
    /// Gets or sets the rate associated with the image asset.
    /// </summary>
    public virtual required int Rate { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the image asset was created.
    /// </summary>
    public virtual required DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the image asset was last updated.
    /// </summary>
    public virtual required DateTimeOffset Updated { get; set; }

    /// <summary>
    /// Gets or sets the associated audio asset.
    /// </summary>
    public virtual AudioAsset AudioAsset { get; set; } = null!;
}
