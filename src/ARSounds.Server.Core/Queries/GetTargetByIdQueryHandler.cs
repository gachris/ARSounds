using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.Repositories.Specifications;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Queries;

/// <summary>
/// Handles the GetTargetByIdQuery and returns the target DTO if found.
/// </summary>
public class GetTargetByIdQueryHandler : IRequestHandler<GetTargetByIdQuery, TargetDto?>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTargetByIdQueryHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTargetByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">Repository for accessing audio assets.</param>
    /// <param name="currentUserService">The current user service to obtain the current user's identifier.</param>
    /// <param name="mapper">The AutoMapper instance used for mapping entities to DTOs.</param>
    /// <param name="logger">The logger instance used for logging informational messages and errors.</param>
    public GetTargetByIdQueryHandler(
        IAudioAssetsRepository audioAssetsRepository,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<GetTargetByIdQueryHandler> logger)
    {
        _audioAssetsRepository = audioAssetsRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the GetTargetByIdQuery request.
    /// Retrieves the audio asset for the current user based on the target ID,
    /// includes associated image asset, and maps the entity to a <see cref="TargetDto"/>.
    /// </summary>
    /// <param name="request">The query request containing the target ID.</param>
    /// <param name="cancellationToken">Token used to cancel the operation if needed.</param>
    /// <returns>
    /// A <see cref="TargetDto"/> representing the target if found; otherwise, null.
    /// </returns>
    public async Task<TargetDto?> Handle(GetTargetByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        _logger.LogInformation("Getting target {TargetId} for user {UserId}", request.TargetId, userId);

        var audioAssetForUserSpecification = new AudioAssetForUserSpecification(request.TargetId, userId)
        {
            Includes = { target => target.ImageAsset }
        };
        var audioAssets = await _audioAssetsRepository.GetBySpecificationAsync(audioAssetForUserSpecification, cancellationToken);
        var audioAsset = audioAssets.SingleOrDefault();

        if (audioAsset is null)
        {
            _logger.LogWarning("Target {TargetId} not found for user {UserId}", request.TargetId, userId);
            return null;
        }

        return _mapper.Map<TargetDto>(audioAsset);
    }

    #endregion
}
