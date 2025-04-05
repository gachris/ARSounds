using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a request to activate a target, including the image data and optional color settings.
/// </summary>
public class ActivateTargetRequest
{
    /// <summary>
    /// Gets or sets the PNG image encoded in base64 format.
    /// The image must be in PNG format.
    /// </summary>
    [JsonPropertyName("png_base64")]
    [Required(ErrorMessage = "Image base64 required. The image must be png.")]
    public string? PngBase64 { get; set; }

    /// <summary>
    /// Gets or sets the hexadecimal color code associated with the target.
    /// </summary>
    [JsonPropertyName("hex_color")]
    public string? HexColor { get; set; }
}
