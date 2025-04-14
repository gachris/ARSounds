using System.ComponentModel.DataAnnotations;

namespace ARSounds.Server.Core.Requests;

/// <summary>
/// Represents a request to create a new target with the required details.
/// This request includes a target name and the audio data encoded in base64 format.
/// </summary>
public class CreateTargetRequest
{
    /// <summary>
    /// Gets or sets the name of the target.
    /// </summary>
    [Required(ErrorMessage = "The target name is required.")]
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the base64-encoded audio data for the target.
    /// </summary>
    [Required(ErrorMessage = "The audio data is required and must be provided in base64 format.")]
    public virtual required string Audio { get; set; }
}
