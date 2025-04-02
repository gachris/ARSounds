using System.Text.Json.Serialization;

namespace ARSounds.Server.Core.Responses;

public class TargetResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; }

    [JsonPropertyName("description")]
    public string Description { get; }

    [JsonPropertyName("title")]
    public string Title { get; }

    [JsonPropertyName("audio_type")]
    public string AudioType { get; }

    [JsonPropertyName("audio_base64")]
    public string AudioBase64 { get; }

    [JsonPropertyName("image_base64")]
    public string? ImageBase64 { get; }

    [JsonPropertyName("vision_target_id")]
    public Guid? VisionTargetId { get; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; }

    [JsonPropertyName("is_trackable")]
    public bool IsTrackable { get; }

    [JsonPropertyName("hex_color")]
    public string? HexColor { get; }

    [JsonPropertyName("rate")]
    public int? Rate { get; }

    [JsonPropertyName("created")]
    public DateTime Created { get; }

    [JsonPropertyName("updated")]
    public DateTime Updated { get; }

    public TargetResponse(Guid id,
                          string description,
                          string title,
                          string audioType,
                          string audioBase64,
                          string? imageBase64,
                          Guid? visionTargetId,
                          bool isActive,
                          bool isTrackable,
                          string? hexColor,
                          int? rate,
                          DateTime created,
                          DateTime updated)
    {
        Id = id;
        Description = description;
        Title = title;
        AudioType = audioType;
        AudioBase64 = audioBase64;
        ImageBase64 = imageBase64;
        VisionTargetId = visionTargetId;
        IsActive = isActive;
        IsTrackable = isTrackable;
        HexColor = hexColor;
        Rate = rate;
        Created = created;
        Updated = updated;
    }
}
