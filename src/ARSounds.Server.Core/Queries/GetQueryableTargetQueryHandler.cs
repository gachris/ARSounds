using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.Repositories.Specifications;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Queries;

/// <summary>
/// Handles the GetTargetsQueryableQuery and returns the list of target DTOs.
/// </summary>
public class GetQueryableTargetQueryHandler : IRequestHandler<GetQueryableTargetQuery, IQueryable<TargetDto>>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetQueryableTargetQueryHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GetQueryableTargetQueryHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">The repository for accessing audio assets.</param>
    /// <param name="currentUserService">The current user service to obtain the current user's identifier.</param>
    /// <param name="mapper">The AutoMapper instance used for mapping entities to DTOs.</param>
    /// <param name="logger">The logger for logging informational messages and errors.</param>
    public GetQueryableTargetQueryHandler(
        IAudioAssetsRepository audioAssetsRepository,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<GetQueryableTargetQueryHandler> logger)
    {
        _audioAssetsRepository = audioAssetsRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the GetTargetsQueryableQuery request.
    /// Retrieves audio assets for the current user, includes associated image assets,
    /// and projects the results to a collection of <see cref="TargetDto"/> objects.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Token used to cancel the operation if needed.</param>
    /// <returns>
    /// An <see cref="IEnumerable{TargetDto}"/> containing the list of target DTOs.
    /// </returns>
    public async Task<IQueryable<TargetDto>> Handle(GetQueryableTargetQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        _logger.LogInformation("Getting targets for user {UserId}", userId);

        var audioAssetForUserSpecification = new AudioAssetForUserSpecification(userId);
        var audioAssetsQueryable = _audioAssetsRepository.GetQueryableBySpecification(audioAssetForUserSpecification);
        var targetDtoQueryable = audioAssetsQueryable.ProjectTo<TargetDto>(_mapper.ConfigurationProvider);

        _logger.LogInformation("Retrieved {Count} targets for user {UserId}", await targetDtoQueryable.CountAsync(cancellationToken), userId);

        return targetDtoQueryable;
    }

    #endregion
}
