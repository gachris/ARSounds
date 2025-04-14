using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.GraphQL.Inputs;
using ARSounds.Server.Core.GraphQL.Payloads;
using ARSounds.Server.Core.GraphQL.Types;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.GraphQL;

/// <summary>
/// GraphQL mutation resolver for target operations.
/// </summary>
public partial class Mutation
{
    #region Fields/Consts

    private readonly IMapper _mapper;
    private readonly ILogger<Mutation> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Mutation"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="logger">The logger instance.</param>
    public Mutation(
        IMapper mapper,
        ILogger<Mutation> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Creates a new target entity.
    /// </summary>
    /// <param name="input">The input containing target creation details.</param>
    /// <param name="targetsService">The service used to manage targets.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A payload containing the created target.</returns>
    [GraphQLDescription("Creates a new target entity.")]
    public virtual async Task<CreateTargetPayload> CreateTargetAsync(
        CreateTargetInput input,
        [Service] ITargetsService targetsService,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(async () =>
        {
            _logger.LogInformation("CreateTargetAsync called.");
            var targetDto = await targetsService.CreateAsync(_mapper.Map<CreateTargetDto>(input), cancellationToken);

            return new CreateTargetPayload
            {
                Target = _mapper.Map<Target>(targetDto)
            };
        });
    }

    /// <summary>
    /// Updates an existing target entity identified by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the target to update.</param>
    /// <param name="input">The input containing updated target details.</param>
    /// <param name="targetsService">The service used to manage targets.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A payload containing the updated target.</returns>
    [GraphQLDescription("Updates a target entity by its ID.")]
    public virtual async Task<UpdateTargetPayload> UpdateTargetAsync(
        Guid id,
        UpdateTargetInput input,
        [Service] ITargetsService targetsService,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(async () =>
        {
            _logger.LogInformation("UpdateTargetAsync called for Id: {TargetId}", id);
            var targetDto = await targetsService.EditAsync(id, _mapper.Map<UpdateTargetDto>(input), cancellationToken);

            return new UpdateTargetPayload
            {
                Target = _mapper.Map<Target>(targetDto)
            };
        });
    }

    /// <summary>
    /// Activates a target entity identified by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the target to activate.</param>
    /// <param name="input">The input containing activation details.</param>
    /// <param name="targetsService">The service used to manage targets.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A payload containing the activated target.</returns>
    [GraphQLDescription("Activates a target entity by its ID.")]
    public virtual async Task<ActivateTargetPayload> ActivateTargetAsync(
        Guid id,
        ActivateTargetInput input,
        [Service] ITargetsService targetsService,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(async () =>
        {
            _logger.LogInformation("ActivateTargetAsync called for Id: {TargetId}", id);
            var targetDto = await targetsService.ActivateAsync(id, _mapper.Map<ActivateTargetDto>(input), cancellationToken);

            return new ActivateTargetPayload
            {
                Target = _mapper.Map<Target>(targetDto)
            };
        });
    }

    /// <summary>
    /// Deactivates a target entity identified by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the target to deactivate.</param>
    /// <param name="targetsService">The service used to manage targets.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A payload containing the deactivated target.</returns>
    [GraphQLDescription("Deactivates a target entity by its ID.")]
    public virtual async Task<DeactivateTargetPayload> DeactivateTargetAsync(
        Guid id,
        [Service] ITargetsService targetsService,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(async () =>
        {
            _logger.LogInformation("DeactivateTargetAsync called for Id: {TargetId}", id);
            var targetDto = await targetsService.DeactivateAsync(id, cancellationToken);

            return new DeactivateTargetPayload
            {
                Target = _mapper.Map<Target>(targetDto)
            };
        });
    }

    /// <summary>
    /// Deletes a target entity identified by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the target to delete.</param>
    /// <param name="targetsService">The service used to manage targets.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A payload indicating whether the deletion was successful.</returns>
    [GraphQLDescription("Deletes a target entity by its ID.")]
    public virtual async Task<DeleteTargetPayload> DeleteTargetAsync(
        Guid id,
        [Service] ITargetsService targetsService,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(async () =>
        {
            _logger.LogInformation("DeleteTargetAsync called for Id: {TargetId}", id);
            var deleted = await targetsService.DeleteAsync(id, cancellationToken);

            return new DeleteTargetPayload
            {
                Success = deleted
            };
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
    /// <exception cref="Exception">Rethrows any caught exception after logging it.</exception>
    private async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing a target mutation.");
            throw;
        }
    }

    #endregion
}
