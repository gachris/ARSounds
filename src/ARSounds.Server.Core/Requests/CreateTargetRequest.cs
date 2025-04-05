using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a request to create a new target with the required details.
/// </summary>
public class CreateTargetRequest
{
    /// <summary>
    /// Gets or sets the description of the target.
    /// </summary>
    [JsonPropertyName("description")]
    [Required(ErrorMessage = "description required.")]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the filename associated with the target's audio.
    /// </summary>
    [JsonPropertyName("filename")]
    [Required(ErrorMessage = "filename required.")]
    public required string Filename { get; set; }

    /// <summary>
    /// Gets or sets the base64-encoded audio data.
    /// </summary>
    [JsonPropertyName("audio_base64")]
    [Required(ErrorMessage = "audio_base64 required.")]
    public required string AudioBase64 { get; set; }

    /// <summary>
    /// Gets or sets the type of the audio (e.g., mp3, wav).
    /// </summary>
    [JsonPropertyName("audio_type")]
    [Required(ErrorMessage = "audio_type required.")]
    public required string AudioType { get; set; }
}
