using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.Repositories.Specifications;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenVision.Api.Target.Resources;
using OpenVision.Shared.Requests;
using OpenVision.Shared.Types;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Handles the UpdateTargetCommand and updates the target.
/// </summary>
public class UpdateTargetCommandHandler : IRequestHandler<UpdateTargetCommand, TargetDto>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly IImageAssetsRepository _imageAssetsRepository;
    private readonly TargetListResource _targetListResource;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateTargetCommandHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTargetCommandHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">The repository for accessing audio assets.</param>
    /// <param name="imageAssetsRepository">The repository for accessing image assets.</param>
    /// <param name="openVisionService">The service for interacting with OpenVision resources.</param>
    /// <param name="currentUserService">The service for obtaining the current user's identifier.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between entities and DTOs.</param>
    /// <param name="logger">The logger for recording informational and error messages.</param>
    public UpdateTargetCommandHandler(
        IAudioAssetsRepository audioAssetsRepository,
        IImageAssetsRepository imageAssetsRepository,
        IOpenVisionService openVisionService,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<UpdateTargetCommandHandler> logger)
    {
        _audioAssetsRepository = audioAssetsRepository;
        _imageAssetsRepository = imageAssetsRepository;
        _targetListResource = openVisionService.GetTargetListResource();
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the UpdateTargetCommand request by updating the target details.
    /// </summary>
    /// <param name="request">The command containing the target ID and update details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A <see cref="TargetDto"/> representing the updated target.</returns>
    public async Task<TargetDto> Handle(UpdateTargetCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        _logger.LogInformation("Editing target {TargetId} for user {UserId}", request.TargetId, userId);

        var audioAssetForUserSpecification = new AudioAssetForUserSpecification(request.TargetId, userId)
        {
            Includes = { target => target.ImageAsset }
        };
        var audioAssets = await _audioAssetsRepository.GetBySpecificationAsync(audioAssetForUserSpecification);
        var audioAsset = audioAssets.SingleOrDefault();

        if (audioAsset is null)
        {
            _logger.LogWarning("Target {TargetId} not found for user {UserId}", request.TargetId, userId);
            throw new ArgumentNullException(nameof(audioAsset));
        }

        audioAsset.Name = request.UpdateTargetDto.Name ?? audioAsset.Name;

        if (audioAsset.ImageAsset is not null)
        {
            var shouldUpdateImageTarget = request.UpdateTargetDto.IsTrackable != null
                && audioAsset.ImageAsset.IsTrackable != request.UpdateTargetDto.IsTrackable;

            if (shouldUpdateImageTarget)
            {
                var updateTrackableRequest = new UpdateTrackableRequest
                {
                    ActiveFlag = audioAsset.ImageAsset.IsTrackable ? ActiveFlag.True : ActiveFlag.False,
                };

                var updateTrackableResponse = await _targetListResource
                    .Update(updateTrackableRequest, audioAsset.ImageAsset.OpenVisionId)
                    .ExecuteAsync(cancellationToken);

                if (updateTrackableResponse.StatusCode is StatusCode.Success)
                {
                    audioAsset.ImageAsset.IsTrackable = request.UpdateTargetDto.IsTrackable ?? audioAsset.ImageAsset.IsTrackable;
                    _logger.LogInformation("Updated trackable status for target {TargetId}", request.TargetId);
                }
                else
                {
                    _logger.LogError("Failed to update trackable status for target {TargetId}: {Errors}", request.TargetId, updateTrackableResponse.Errors);
                }
            }

            audioAsset.ImageAsset.Color = request.UpdateTargetDto.Color ?? audioAsset.ImageAsset.Color;
            await _imageAssetsRepository.UpdateAsync(audioAsset.ImageAsset, cancellationToken);
            _logger.LogInformation("Updated image asset for target {TargetId}", request.TargetId);
        }

        await _audioAssetsRepository.UpdateAsync(audioAsset, cancellationToken);
        _logger.LogInformation("Edited target {TargetId} for user {UserId}", request.TargetId, userId);

        return _mapper.Map<TargetDto>(audioAsset);
    }

    #endregion
}
