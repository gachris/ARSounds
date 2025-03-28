using System.Text.Json.Serialization;

namespace ARSounds.Web.Core.Requests;

public class UpdateTargetRequest
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("is_trackable")]
    public bool? IsTrackable { get; set; }

    [JsonPropertyName("hex_color")]
    public string? HexColor { get; set; }
}
