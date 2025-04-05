using System.Text.Json.Serialization;

namespace ARSounds.Server.Core.Responses;

/// <summary>
/// Represents the response model for a target.
/// </summary>
public class TargetResponse
{
    /// <summary>
    /// Gets the unique identifier of the target.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Gets the description of the target.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; }

    /// <summary>
    /// Gets the title of the target.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; }

    /// <summary>
    /// Gets the audio type of the target.
    /// </summary>
    [JsonPropertyName("audio_type")]
    public string AudioType { get; }

    /// <summary>
    /// Gets the base64-encoded audio data.
    /// </summary>
    [JsonPropertyName("audio_base64")]
    public string AudioBase64 { get; }

    /// <summary>
    /// Gets the base64-encoded image data.
    /// </summary>
    [JsonPropertyName("image_base64")]
    public string? ImageBase64 { get; }

    /// <summary>
    /// Gets the unique identifier associated with the vision target.
    /// </summary>
    [JsonPropertyName("vision_target_id")]
    public Guid? VisionTargetId { get; }

    /// <summary>
    /// Gets a value indicating whether the target is active.
    /// </summary>
    [JsonPropertyName("is_active")]
    public bool IsActive { get; }

    /// <summary>
    /// Gets a value indicating whether the target is trackable.
    /// </summary>
    [JsonPropertyName("is_trackable")]
    public bool IsTrackable { get; }

    /// <summary>
    /// Gets the hexadecimal color code associated with the target.
    /// </summary>
    [JsonPropertyName("hex_color")]
    public string? HexColor { get; }

    /// <summary>
    /// Gets the rating of the target.
    /// </summary>
    [JsonPropertyName("rate")]
    public int? Rate { get; }

    /// <summary>
    /// Gets the creation date and time of the target.
    /// </summary>
    [JsonPropertyName("created")]
    public DateTime Created { get; }

    /// <summary>
    /// Gets the last update date and time of the target.
    /// </summary>
    [JsonPropertyName("updated")]
    public DateTime Updated { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetResponse"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the target.</param>
    /// <param name="description">The description of the target.</param>
    /// <param name="title">The title of the target.</param>
    /// <param name="audioType">The audio type of the target.</param>
    /// <param name="audioBase64">The base64-encoded audio data.</param>
    /// <param name="imageBase64">The base64-encoded image data.</param>
    /// <param name="visionTargetId">The unique identifier of the vision target.</param>
    /// <param name="isActive">A value indicating whether the target is active.</param>
    /// <param name="isTrackable">A value indicating whether the target is trackable.</param>
    /// <param name="hexColor">The hexadecimal color code of the target.</param>
    /// <param name="rate">The rating of the target.</param>
    /// <param name="created">The creation date and time of the target.</param>
    /// <param name="updated">The last update date and time of the target.</param>
    public TargetResponse(
        Guid id,
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
