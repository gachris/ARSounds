using ARSounds.EntityFramework.Entities;

namespace ARSounds.Server.Core.Repositories.Specifications;

/// <summary>
/// Represents a specification for filtering <see cref="AudioAsset"/> entities
/// based on user-specific criteria.
/// </summary>
public class AudioAssetForUserSpecification : BaseSpecification<AudioAsset>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioAssetForUserSpecification"/> class 
    /// with a filter that selects audio assets associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    public AudioAssetForUserSpecification(string userId)
        : base(target => target.UserId == userId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioAssetForUserSpecification"/> class 
    /// with a filter that selects a specific audio asset associated with the specified user.
    /// </summary>
    /// <param name="targetId">The unique identifier of the audio asset.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    public AudioAssetForUserSpecification(Guid targetId, string userId)
        : base(target => target.Id == targetId && target.UserId == userId)
    {
    }
}