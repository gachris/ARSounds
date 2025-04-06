using ARSounds.EntityFramework.Entities;

namespace ARSounds.Server.Core.Contracts;

/// <summary>
/// Provides asynchronous methods to access and manipulate audio asset data.
/// </summary>
public interface IAudioAssetsRepository
{
    /// <summary>
    /// Asynchronously retrieves a queryable collection of <see cref="AudioAsset"/> entities.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains an
    /// <see cref="IQueryable{AudioAsset}"/> that can be further filtered or projected.
    /// </returns>
    Task<IQueryable<AudioAsset>> GetAsync();

    /// <summary>
    /// Asynchronously creates a new audio asset.
    /// </summary>
    /// <param name="target">The <see cref="AudioAsset"/> entity to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> that resolves to <c>true</c> if the creation was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> CreateAsync(AudioAsset target, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing audio asset.
    /// </summary>
    /// <param name="target">The <see cref="AudioAsset"/> entity with updated values.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> that resolves to <c>true</c> if the update was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> UpdateAsync(AudioAsset target, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously removes an audio asset.
    /// </summary>
    /// <param name="image">The <see cref="AudioAsset"/> entity to remove.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> that resolves to <c>true</c> if the removal was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> RemoveAsync(AudioAsset image, CancellationToken cancellationToken = default);
}