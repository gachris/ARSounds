using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ARSounds.Web.Core.Requests;

public class UpdateTargetRequest
{
    [Required]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [Required]
    [JsonPropertyName("is_trackable")]
    public bool? IsTrackable { get; set; }

    [Required]
    [JsonPropertyName("hex_color")]
    public string? HexColor { get; set; }
}
