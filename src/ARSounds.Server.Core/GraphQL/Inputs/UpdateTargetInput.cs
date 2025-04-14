namespace ARSounds.Server.Core.GraphQL.Inputs;

/// <summary>
/// Represents the data used to update a target, with each field being optional.
/// </summary>
[GraphQLDescription("Represents the data used to update a target, with each field being optional.")]
public record UpdateTargetInput
{
    /// <summary>
    /// Gets or sets the updated name of the target.
    /// </summary>
    [GraphQLDescription("The updated name of the target.")]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target should be trackable.
    /// </summary>
    [GraphQLDescription("A value indicating whether the target should be trackable.")]
    public virtual bool? IsTrackable { get; set; }

    /// <summary>
    /// Gets or sets the updated hexadecimal color code for the target.
    /// </summary>
    [GraphQLDescription("The updated hexadecimal color code for the target.")]
    public virtual string? Color { get; set; }
}
