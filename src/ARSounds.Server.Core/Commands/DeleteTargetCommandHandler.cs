using ARSounds.Server.Core.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenVision.Api.Target.Resources;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Handles the DeleteTargetCommand and deletes the target.
/// </summary>
public class DeleteTargetCommandHandler : IRequestHandler<DeleteTargetCommand, bool>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly TargetListResource _targetListResource;
    private readonly ILogger<DeleteTargetCommandHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTargetCommandHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">The repository for accessing audio assets.</param>
    /// <param name="openVisionService">The service for interacting with OpenVision resources.</param>
    /// <param name="logger">The logger for recording informational and error messages.</param>
    /// <param name="currentUserService">The service for obtaining the current user's identifier.</param>
    public DeleteTargetCommandHandler(
        IAudioAssetsRepository audioAssetsRepository,
        IOpenVisionService openVisionService,
        ILogger<DeleteTargetCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _audioAssetsRepository = audioAssetsRepository;
        _targetListResource = openVisionService.GetTargetListResource();
        _logger = logger;
        _currentUserService = currentUserService;
    }

    #region Methods

    /// <summary>
    /// Handles the DeleteTargetCommand request by deleting the target.
    /// </summary>
    /// <param name="request">The command containing the target ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A boolean value indicating whether the deletion was successful.</returns>
    public async Task<bool> Handle(DeleteTargetCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        _logger.LogInformation("Deleting target {TargetId} for user {UserId}", request.TargetId, userId);

        var audioAssetsQueryable = await _audioAssetsRepository.GetAsync();

        var target = await audioAssetsQueryable
            .Where(x => x.Id == request.TargetId && x.UserId == userId)
            .Include(a => a.ImageAsset)
            .SingleOrDefaultAsync(cancellationToken);

        if (target is null)
        {
            _logger.LogWarning("Target {TargetId} not found for user {UserId}", request.TargetId, userId);
            return false;
        }

        if (target.ImageAsset is not null)
        {
            var deleteResponse = await _targetListResource.Delete(target.ImageAsset.OpenVisionId)
                .ExecuteAsync(cancellationToken);

            if (deleteResponse.StatusCode is OpenVision.Shared.StatusCode.Failed)
            {
                _logger.LogError("Failed to delete trackable image for target {TargetId}: {Errors}", request.TargetId, deleteResponse.Errors);
            }
            else
            {
                _logger.LogInformation("Deleted trackable image for target {TargetId}", request.TargetId);
            }
        }

        var result = await _audioAssetsRepository.RemoveAsync(target, cancellationToken);
        _logger.LogInformation("Deleted target {TargetId} for user {UserId}", request.TargetId, userId);

        return result;
    }

    #endregion
}
