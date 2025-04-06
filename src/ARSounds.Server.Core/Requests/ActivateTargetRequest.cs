using System.ComponentModel.DataAnnotations;

namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a request to activate a target.
/// This request includes the target's PNG image (encoded in base64 format) and an optional hexadecimal color code.
/// </summary>
public class ActivateTargetRequest
{
    /// <summary>
    /// Gets or sets the PNG image encoded in base64 format.
    /// The image must be in PNG format.
    /// </summary>
    [Required(ErrorMessage = "A base64-encoded PNG image is required.")]
    public virtual required string Image { get; set; }

    /// <summary>
    /// Gets or sets the optional hexadecimal color code associated with the target.
    /// </summary>
    public virtual string? Color { get; set; }
}
