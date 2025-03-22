using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ARSounds.Web.Core.Requests;

public class ActivateTargetRequest
{
    [JsonPropertyName("image_base64")]
    [Required(ErrorMessage = "Image base64 required. The image must be png.")]
    public string? ImageBase64 { get; set; }

    [JsonPropertyName("hex_color")]
    public string? HexColor { get; set; }
}