using ARSounds.Server.Core.Dtos;

namespace ARSounds.Server.Core.Contracts;

/// <summary>
/// Defines a contract for managing target entities, including operations for creation, retrieval, editing,
/// activation, deactivation, and deletion.
/// </summary>
public interface ITargetsService
{
    /// <summary>
    /// Gets a queryable collection of target DTOs for further composition (e.g., filtering, sorting, paging).
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IQueryable{TargetDto}"/> of target DTOs.</returns>
    Task<IQueryable<TargetDto>> GetQueryableAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all target DTOs.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>An enumerable collection of target DTOs.</returns>
    Task<IEnumerable<TargetDto>> GetAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a specific target DTO by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the target.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// The target DTO corresponding to the provided identifier if it exists; otherwise, <c>null</c>.
    /// </returns>
    Task<TargetDto?> GetAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new target.
    /// </summary>
    /// <param name="createTargetDto">A DTO containing the details for the new target.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>The newly created target DTO.</returns>
    Task<TargetDto> CreateAsync(CreateTargetDto createTargetDto, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing target.
    /// </summary>
    /// <param name="id">The unique identifier of the target to update.</param>
    /// <param name="updateTargetDto">A DTO containing the updated target details.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>The updated target DTO.</returns>
    Task<TargetDto> EditAsync(Guid id, UpdateTargetDto updateTargetDto, CancellationToken cancellationToken);

    /// <summary>
    /// Activates a target by creating and associating a new trackable image asset.
    /// </summary>
    /// <param name="id">The unique identifier of the target to activate.</param>
    /// <param name="activateTargetDto">A DTO containing the activation details.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// The target DTO updated with an activated trackable image asset.
    /// </returns>
    Task<TargetDto> ActivateAsync(Guid id, ActivateTargetDto activateTargetDto, CancellationToken cancellationToken);

    /// <summary>
    /// Deactivates a target by removing its associated trackable image asset.
    /// </summary>
    /// <param name="id">The unique identifier of the target to deactivate.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// The target DTO updated with the trackable image asset removed.
    /// </returns>
    Task<TargetDto> DeactivateAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a target and its associated image asset, if present.
    /// </summary>
    /// <param name="id">The unique identifier of the target to delete.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>
    /// <c>true</c> if the deletion was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
