using Newtonsoft.Json;

namespace ARSounds.Core.Targets;

public class Target
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("audio_type")]
    public string AudioType { get; set; }

    [JsonProperty("audio_base64")]
    public string AudioBase64 { get; set; }

    [JsonProperty("image_base64")]
    public string? ImageBase64 { get; set; }

    [JsonProperty("vision_target_id")]
    public Guid? VisionTargetId { get; set; }

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("is_trackable")]
    public bool IsTrackable { get; set; }

    [JsonProperty("hex_color")]
    public string? HexColor { get; set; }

    [JsonProperty("rate")]
    public int? Rate { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }

    [JsonProperty("updated")]
    public DateTime Updated { get; set; }
}
