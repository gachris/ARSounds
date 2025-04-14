using ARSounds.EntityFramework.Entities;
using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.Helpers;
using ARSounds.Server.Core.Repositories.Specifications;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenVision.Api.Target.Resources;
using OpenVision.Shared.Requests;
using OpenVision.Shared.Types;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Handles the ActivateTargetCommand and activates the target.
/// </summary>
public class ActivateTargetCommandHandler : IRequestHandler<ActivateTargetCommand, TargetDto>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly IImageAssetsRepository _imageAssetsRepository;
    private readonly TargetListResource _targetListResource;
    private readonly IMapper _mapper;
    private readonly ILogger<ActivateTargetCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivateTargetCommandHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">The repository for accessing audio assets.</param>
    /// <param name="imageAssetsRepository">The repository for accessing image assets.</param>
    /// <param name="openVisionService">The service for interacting with OpenVision resources.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between entities and DTOs.</param>
    /// <param name="logger">The logger for recording informational and error messages.</param>
    /// <param name="currentUserService">The current user service to obtain the current user's identifier.</param>
    public ActivateTargetCommandHandler(
        IAudioAssetsRepository audioAssetsRepository,
        IImageAssetsRepository imageAssetsRepository,
        IOpenVisionService openVisionService,
        IMapper mapper,
        ILogger<ActivateTargetCommandHandler> logger,
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
    /// Handles the ActivateTargetCommand request by activating the target.
    /// The current user's identifier is retrieved from the <see cref="ICurrentUserService"/>.
    /// </summary>
    /// <param name="request">The command containing the target ID and activation details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A <see cref="TargetDto"/> representing the activated target.</returns>
    public async Task<TargetDto> Handle(ActivateTargetCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        _logger.LogInformation("Activating target {TargetId} for user {UserId}", request.TargetId, userId);

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

        var currentTime = DateTime.UtcNow;

        var openVisionImage = Utils.GetOpenVisionImage(request.ActivateTargetDto.Image);

        var encodedName = string.Concat(audioAsset.Name, "|", currentTime).Base64Encode();

        if (audioAsset.ImageAsset is not null)
        {
            var updateTrackableRequest = new UpdateTrackableRequest
            {
                Name = encodedName,
                Image = openVisionImage,
                Width = 1,
                ActiveFlag = ActiveFlag.True,
                Metadata = encodedName
            };

            var updateTrackableResponse = await _targetListResource
                .Update(updateTrackableRequest, audioAsset.ImageAsset.OpenVisionId)
                .ExecuteAsync(cancellationToken);

            if (updateTrackableResponse.StatusCode is StatusCode.Failed)
            {
                _logger.LogError("Failed to post trackable target for target {TargetId}: {Errors}", request.TargetId, updateTrackableResponse.Errors);
                throw new Exception("Failed to update trackable target.");
            }

            _logger.LogInformation("Updating existing trackable image for target {TargetId}", request.TargetId);

            audioAsset.ImageAsset.Image = Utils.GetARSoundsImage(openVisionImage);
            audioAsset.ImageAsset.Color = request.ActivateTargetDto.Color ?? "#000000";
            audioAsset.ImageAsset.IsTrackable = true;
            audioAsset.ImageAsset.Rate = 5;
            audioAsset.ImageAsset.Updated = currentTime;

            await _imageAssetsRepository.UpdateAsync(audioAsset.ImageAsset, cancellationToken);
        }
        else
        {
            var postTrackableRequest = new PostTrackableRequest
            {
                Name = encodedName,
                Image = openVisionImage,
                Width = 1,
                ActiveFlag = ActiveFlag.True,
                Metadata = encodedName
            };

            var postTrackableResponse = await _targetListResource
                .Insert(postTrackableRequest)
                .ExecuteAsync(cancellationToken);

            if (postTrackableResponse.StatusCode is StatusCode.Failed)
            {
                _logger.LogError("Failed to post trackable target for target {TargetId}: {Errors}", request.TargetId, postTrackableResponse.Errors);
                throw new Exception("Failed to post trackable target.");
            }

            var imageAsset = new ImageAsset
            {
                Id = Guid.NewGuid(),
                AudioAssetId = audioAsset.Id,
                Image = Utils.GetARSoundsImage(openVisionImage),
                OpenVisionId = postTrackableResponse.Response.Result.ToString(),
                Color = request.ActivateTargetDto.Color ?? "#000000",
                IsTrackable = true,
                Rate = 5,
                Created = currentTime,
                Updated = currentTime
            };

            await _imageAssetsRepository.CreateAsync(imageAsset, cancellationToken);
        }

        _logger.LogInformation("Activated target {TargetId} for user {UserId}", request.TargetId, userId);

        return _mapper.Map<TargetDto>(audioAsset);
    }

    #endregion
}
