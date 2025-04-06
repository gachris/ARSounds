namespace ARSounds.Server.Core.GraphQL.Inputs;

/// <summary>
/// Represents the data required to create a new target.
/// </summary>
[GraphQLDescription("Represents the data required to create a new target.")]
public record CreateTargetInput
{
    /// <summary>
    /// Gets or sets the name of the target.
    /// </summary>
    [GraphQLDescription("The name of the target.")]
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the audio data encoded in base64 format.
    /// </summary>
    [GraphQLDescription("The audio data encoded in base64 format.")]
    public virtual required string Audio { get; set; }
}
