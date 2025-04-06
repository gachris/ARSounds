using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Queries;

/// <summary>
/// Handles the GetTargetsQuery and returns the list of target DTOs.
/// </summary>
public class GetTargetsQueryHandler : IRequestHandler<GetTargetsQuery, IQueryable<TargetDto>>
{
    #region Fields/Consts

    private readonly IAudioAssetsRepository _audioAssetsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTargetsQueryHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTargetsQueryHandler"/> class.
    /// </summary>
    /// <param name="audioAssetsRepository">The repository for accessing audio assets.</param>
    /// <param name="currentUserService">The current user service to obtain the current user's identifier.</param>
    /// <param name="mapper">The AutoMapper instance used for mapping entities to DTOs.</param>
    /// <param name="logger">The logger for logging informational messages and errors.</param>
    public GetTargetsQueryHandler(
        IAudioAssetsRepository audioAssetsRepository,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<GetTargetsQueryHandler> logger)
    {
        _audioAssetsRepository = audioAssetsRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the GetTargetsQuery request.
    /// Retrieves audio assets for the current user, includes associated image assets,
    /// and projects the results to a collection of <see cref="TargetDto"/> objects.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Token used to cancel the operation if needed.</param>
    /// <returns>
    /// An <see cref="IQueryable{TargetDto}"/> containing the list of target DTOs.
    /// </returns>
    public async Task<IQueryable<TargetDto>> Handle(GetTargetsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        _logger.LogInformation("Getting targets for user {UserId}", userId);

        var audioAssetsQueryable = await _audioAssetsRepository.GetAsync();

        var audioAssets = audioAssetsQueryable
            .Include(a => a.ImageAsset)
            .Where(x => x.UserId == userId);

        var targets = audioAssets.ProjectTo<TargetDto>(_mapper.ConfigurationProvider);

        _logger.LogInformation("Retrieved {Count} targets for user {UserId}", targets.Count(), userId);

        return await Task.FromResult(targets);
    }

    #endregion
}
