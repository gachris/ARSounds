using ARSounds.Server.Core.GraphQL.Types;

namespace ARSounds.Server.Core.GraphQL.Payloads;

/// <summary>
/// Represents the payload returned when a target is created.
/// </summary>
[GraphQLDescription("Represents the payload returned when a target is created.")]
public record CreateTargetPayload
{
    /// <summary>
    /// Gets or sets the created target.
    /// </summary>
    [GraphQLDescription("The created target.")]
    public virtual required Target Target { get; set; }
}
