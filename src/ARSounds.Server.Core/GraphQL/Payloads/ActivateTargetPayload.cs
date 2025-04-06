using ARSounds.Server.Core.GraphQL.Types;

namespace ARSounds.Server.Core.GraphQL.Payloads;

/// <summary>
/// Represents the payload returned when a target is activated.
/// </summary>
[GraphQLDescription("Represents the payload returned when a target is activated.")]
public record ActivateTargetPayload
{
    /// <summary>
    /// Gets or sets the activated target.
    /// </summary>
    [GraphQLDescription("The activated target.")]
    public virtual required Target Target { get; set; }
}
