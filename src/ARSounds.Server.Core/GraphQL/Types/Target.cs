namespace ARSounds.Server.Core.GraphQL.Types;

/// <summary>
/// Represents a target entity exposed via the GraphQL API.
/// </summary>
[GraphQLDescription("Represents a target entity with audio, image, and tracking details.")]
public record Target
{
    /// <summary>
    /// Gets or sets the unique identifier of the target.
    /// </summary>
    [GraphQLDescription("The unique identifier of the target.")]
    [ID]
    public virtual required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the target.
    /// </summary>
    [GraphQLDescription("The name of the target.")]
    public virtual required string Name { get; set; }

    /// <summary>
    /// Gets or sets the base64-encoded audio data.
    /// </summary>
    [GraphQLDescription("The base64-encoded audio data of the target.")]
    public virtual required string Audio { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target is active.
    /// </summary>
    [GraphQLDescription("Indicates whether the target is active.")]
    public virtual required bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the base64-encoded image data.
    /// </summary>
    [GraphQLDescription("The base64-encoded image data of the target, if available.")]
    public virtual string? Image { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier associated with the vision target.
    /// </summary>
    [GraphQLDescription("The unique identifier associated with the vision target, if available.")]
    public virtual string? OpenVisionId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the target is trackable.
    /// </summary>
    [GraphQLDescription("Indicates whether the target is trackable, if available.")]
    public virtual bool? IsTrackable { get; set; }

    /// <summary>
    /// Gets or sets the hexadecimal color code associated with the target.
    /// </summary>
    [GraphQLDescription("The hexadecimal color code associated with the target, if available.")]
    public virtual string? Color { get; set; }

    /// <summary>
    /// Gets or sets the rating of the target.
    /// </summary>
    [GraphQLDescription("The rating of the target, if available.")]
    public virtual int? Rate { get; set; }

    /// <summary>
    /// Gets or sets the creation date and time of the target.
    /// </summary>
    [GraphQLDescription("The creation date and time of the target.")]
    public virtual required DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets the last update date and time of the target.
    /// </summary>
    [GraphQLDescription("The last update date and time of the target.")]
    public virtual required DateTimeOffset Updated { get; set; }
}
