using ARSounds.Server.Core.GraphQL.Types;

namespace ARSounds.Server.Core.GraphQL.Payloads;

/// <summary>
/// Represents the payload returned when a target is updated.
/// </summary>
[GraphQLDescription("Represents the payload returned when a target is updated.")]
public record UpdateTargetPayload
{
    /// <summary>
    /// Gets or sets the updated target.
    /// </summary>
    [GraphQLDescription("The updated target.")]
    public virtual required Target Target { get; set; }
}
