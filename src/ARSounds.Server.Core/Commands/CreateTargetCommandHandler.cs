using ARSounds.EntityFramework.Entities;
using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.Helpers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Handles the CreateTargetCommand and creates a new target.
/// </summary>
public class CreateTargetCommandHandler : IRequestHandler<CreateTargetCommand, TargetDto>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTargetCommandHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTargetCommandHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">The repository for accessing audio assets.</param>
    /// <param name="currentUserService">The service for obtaining the current user's identifier.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between entities and DTOs.</param>
    /// <param name="logger">The logger for recording informational and error messages.</param>
    public CreateTargetCommandHandler(
        IAudioAssetsRepository audioAssetsRepository,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<CreateTargetCommandHandler> logger)
    {
        _audioAssetsRepository = audioAssetsRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the CreateTargetCommand request to create a new target.
    /// </summary>
    /// <param name="request">The command containing target creation details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A <see cref="TargetDto"/> representing the newly created target.</returns>
    public async Task<TargetDto> Handle(CreateTargetCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        _logger.LogInformation("Creating target for user {UserId}", userId);

        var currentTime = DateTime.UtcNow;
        var audioAsset = new AudioAsset
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.CreateTargetDto.Name,
            Audio = request.CreateTargetDto.Audio.Base64ToByteArray(),
            Created = currentTime,
            Updated = currentTime
        };

        await _audioAssetsRepository.CreateAsync(audioAsset, cancellationToken);
        _logger.LogInformation("Created target {TargetId} for user {UserId}", audioAsset.Id, userId);

        return _mapper.Map<TargetDto>(audioAsset);
    }

    #endregion
}
