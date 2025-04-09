using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.GraphQL.Types;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.GraphQL;

/// <summary>
/// GraphQL query resolver for target operations.
/// </summary>
public partial class Query
{
    #region Fields/Consts

    private readonly IMapper _mapper;
    private readonly ILogger<Query> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger instance.</param>
    public Query(
        IMapper mapper,
        ILogger<Query> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Retrieves a paginated, filtered, and sorted list of target entities.
    /// </summary>
    /// <param name="targetsService">The targets service to retrieve target data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IQueryable{Target}"/> representing the target entities.</returns>
    [GraphQLDescription("Retrieves a paginated, filtered, and sorted list of target entities.")]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public virtual async Task<IQueryable<Target>> GetTargets(
        [Service] ITargetsService targetsService,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            _logger.LogInformation("GetTargets called.");
            var targetDtoQuery = await targetsService.GetQueryableAsync(cancellationToken);
            _logger.LogInformation("Retrieved {Count} target(s)", await targetDtoQuery.CountAsync(cancellationToken));
            return targetDtoQuery.ProjectTo<Target>(_mapper.ConfigurationProvider);
        });
    }

    /// <summary>
    /// Retrieves a specific target entity by its unique identifier.
    /// Optional filtering is applied based on the provided resolver context.
    /// </summary>
    /// <param name="id">The unique identifier of the target.</param>
    /// <param name="targetsService">The targets service to retrieve target data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The matching <see cref="Target"/> entity if found; otherwise, <c>null</c>.</returns>
    [GraphQLDescription("Retrieves a specific target entity by its unique identifier.")]
    public virtual async Task<Target?> GetTarget(
        Guid id,
        [Service] ITargetsService targetsService,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            _logger.LogInformation("GetTarget called with Id: {TargetId}", id);
            var targetDto = await targetsService.GetAsync(id, cancellationToken);
            if (targetDto == null)
            {
                _logger.LogWarning("No target found with Id: {TargetId}", id);
                return null;
            }
            return _mapper.Map<Target>(targetDto);
        });
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Executes the provided asynchronous function within a try-catch block to log and rethrow errors.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="action">The asynchronous function to execute.</param>
    /// <returns>The result of the function.</returns>
    /// <exception cref="Exception">Rethrows any caught exceptions.</exception>
    private async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing the GraphQL query.");
            throw;
        }
    }

    #endregion
}
