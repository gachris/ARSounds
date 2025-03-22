using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ARSounds.Web.Core.Requests;

public class CreateTargetRequest
{
    [JsonPropertyName("description")]
    [Required(ErrorMessage = "description required.")]
    public string? Description { get; set; }

    [JsonPropertyName("filename")]
    [Required(ErrorMessage = "filename required.")]
    public string? Filename { get; set; }

    [JsonPropertyName("audio_base64")]
    [Required(ErrorMessage = "audio_base64 required.")]
    public string? AudioBase64 { get; set; }

    [JsonPropertyName("audio_type")]
    [Required(ErrorMessage = "audio_type required.")]
    public string? AudioType { get; set; }
}