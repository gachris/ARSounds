using System.Text.Json.Serialization;

namespace ARSounds.ApiClient.Dtos;

public class TargetDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("audio_type")]
    public required string AudioType { get; set; }

    [JsonPropertyName("audio_base64")]
    public required string AudioBase64 { get; set; }

    [JsonPropertyName("image_base64")]
    public string? ImageBase64 { get; set; }

    [JsonPropertyName("vision_target_id")]
    public Guid? VisionTargetId { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("is_trackable")]
    public bool IsTrackable { get; set; }

    [JsonPropertyName("hex_color")]
    public string? HexColor { get; set; }

    [JsonPropertyName("rate")]
    public int? Rate { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }
}