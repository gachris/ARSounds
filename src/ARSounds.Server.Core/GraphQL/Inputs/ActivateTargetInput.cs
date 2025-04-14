namespace ARSounds.Server.Core.GraphQL.Inputs;

/// <summary>
/// Represents the data required to activate a target, including a PNG image encoded in base64 format and an optional hexadecimal color code.
/// </summary>
[GraphQLDescription("Represents the data required to activate a target, including a PNG image encoded in base64 format and an optional hexadecimal color code.")]
public record ActivateTargetInput
{
    /// <summary>
    /// Gets or sets the PNG image encoded in base64 format. The image must be in PNG format.
    /// </summary>
    [GraphQLDescription("The PNG image encoded in base64 format. The image must be in PNG format.")]
    public virtual required string Image { get; set; }

    /// <summary>
    /// Gets or sets an optional hexadecimal color code associated with the target.
    /// </summary>
    [GraphQLDescription("An optional hexadecimal color code associated with the target.")]
    public virtual string? Color { get; set; }
}
