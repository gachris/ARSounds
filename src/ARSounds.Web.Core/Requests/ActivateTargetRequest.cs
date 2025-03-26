using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ARSounds.Web.Core.Requests;

public class ActivateTargetRequest
{
    [JsonPropertyName("png_base64")]
    [Required(ErrorMessage = "Image base64 required. The image must be png.")]
    public string? PngBase64 { get; set; }

    [JsonPropertyName("hex_color")]
    public string? HexColor { get; set; }
}