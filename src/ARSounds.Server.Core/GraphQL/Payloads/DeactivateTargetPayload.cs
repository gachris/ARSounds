using ARSounds.Server.Core.GraphQL.Types;

namespace ARSounds.Server.Core.GraphQL.Payloads;

/// <summary>
/// Represents the payload returned when a target is deactivated.
/// </summary>
[GraphQLDescription("Represents the payload returned when a target is deactivated.")]
public record DeactivateTargetPayload
{
    /// <summary>
    /// Gets or sets the deactivated target.
    /// </summary>
    [GraphQLDescription("The deactivated target.")]
    public virtual required Target Target { get; set; }
}
