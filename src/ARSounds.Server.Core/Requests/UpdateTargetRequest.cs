using System.Text.Json.Serialization;

namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a request to update a target with optional fields.
/// </summary>
public class UpdateTargetRequest
{
    /// <summary>
    /// Gets or sets the updated description of the target.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target should be trackable.
    /// </summary>
    [JsonPropertyName("is_trackable")]
    public bool? IsTrackable { get; set; }

    /// <summary>
    /// Gets or sets the updated hexadecimal color code for the target.
    /// </summary>
    [JsonPropertyName("hex_color")]
    public string? HexColor { get; set; }
}
