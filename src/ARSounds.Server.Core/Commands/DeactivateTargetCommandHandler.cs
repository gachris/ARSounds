using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.Repositories.Specifications;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenVision.Api.Target.Resources;
using OpenVision.Shared.Types;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Handles the DeactivateTargetCommand and deactivates the target.
/// </summary>
public class DeactivateTargetCommandHandler : IRequestHandler<DeactivateTargetCommand, TargetDto>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly IImageAssetsRepository _imageAssetsRepository;
    private readonly TargetListResource _targetListResource;
    private readonly IMapper _mapper;
    private readonly ILogger<DeactivateTargetCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="DeactivateTargetCommandHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">The repository for accessing audio assets.</param>
    /// <param name="imageAssetsRepository">The repository for accessing image assets.</param>
    /// <param name="openVisionService">The service for interacting with OpenVision resources.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    /// <param name="logger">The logger for recording informational and error messages.</param>
    /// <param name="currentUserService">The current user service to obtain the current user's identifier.</param>
    public DeactivateTargetCommandHandler(
        IAudioAssetsRepository audioAssetsRepository,
        IImageAssetsRepository imageAssetsRepository,
        IOpenVisionService openVisionService,
        IMapper mapper,
        ILogger<DeactivateTargetCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _audioAssetsRepository = audioAssetsRepository;
        _imageAssetsRepository = imageAssetsRepository;
        _targetListResource = openVisionService.GetTargetListResource();
        _mapper = mapper;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    #region Methods

    /// <summary>
    /// Handles the DeactivateTargetCommand request by deactivating the target.
    /// </summary>
    /// <param name="request">The command containing the target ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A <see cref="TargetDto"/> representing the deactivated target.</returns>
    public async Task<TargetDto> Handle(DeactivateTargetCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        _logger.LogInformation("Deactivating target {TargetId} for user {UserId}", request.TargetId, userId);

        var audioAssetForUserSpecification = new AudioAssetForUserSpecification(request.TargetId, userId)
        {
            Includes = { target => target.ImageAsset }
        };
        var audioAssets = await _audioAssetsRepository.GetBySpecificationAsync(audioAssetForUserSpecification, cancellationToken);
        var audioAsset = audioAssets.SingleOrDefault();

        if (audioAsset is null)
        {
            _logger.LogWarning("Target {TargetId} not found for user {UserId}", request.TargetId, userId);
            throw new ArgumentNullException(nameof(audioAsset));
        }

        var imageAsset = audioAsset.ImageAsset;
        if (imageAsset is null)
        {
            _logger.LogInformation("No image asset found for target {TargetId}, skipping deactivation.", request.TargetId);
            return _mapper.Map<TargetDto>(audioAsset);
        }

        var deleteResponse = await _targetListResource.Delete(imageAsset.OpenVisionId)
            .ExecuteAsync(cancellationToken);

        if (deleteResponse.StatusCode is StatusCode.Failed)
        {
            _logger.LogError("Failed to delete trackable image for target {TargetId}: {Errors}", request.TargetId, deleteResponse.Errors);
        }
        else
        {
            _logger.LogInformation("Deleted trackable image for target {TargetId}", request.TargetId);
        }

        await _imageAssetsRepository.RemoveAsync(imageAsset, cancellationToken);

        _logger.LogInformation("Deactivated target {TargetId} for user {UserId}", request.TargetId, userId);

        return _mapper.Map<TargetDto>(audioAsset);
    }

    #endregion
}
