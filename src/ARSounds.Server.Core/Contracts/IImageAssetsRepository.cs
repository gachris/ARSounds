using ARSounds.EntityFramework.Entities;

namespace ARSounds.Server.Core.Contracts;

/// <summary>
/// Provides asynchronous methods to manage image asset entities.
/// </summary>
public interface IImageAssetsRepository
{
    /// <summary>
    /// Asynchronously retrieves a queryable collection of <see cref="ImageAsset"/> entities.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains an 
    /// <see cref="IQueryable{ImageAsset}"/> collection that can be further filtered or projected.
    /// </returns>
    Task<IQueryable<ImageAsset>> GetAsync();

    /// <summary>
    /// Asynchronously creates a new image asset.
    /// </summary>
    /// <param name="image">The <see cref="ImageAsset"/> entity to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation that resolves to <c>true</c> if 
    /// the creation was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> CreateAsync(ImageAsset image, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing image asset.
    /// </summary>
    /// <param name="image">The <see cref="ImageAsset"/> entity with updated information.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation that resolves to <c>true</c> if 
    /// the update was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> UpdateAsync(ImageAsset image, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously removes an image asset.
    /// </summary>
    /// <param name="image">The <see cref="ImageAsset"/> entity to remove.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation that resolves to <c>true</c> if 
    /// the removal was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> RemoveAsync(ImageAsset image, CancellationToken cancellationToken = default);
}
