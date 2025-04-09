using ARSounds.Server.Core.Commands;
using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Services;

/// <summary>
/// Provides implementation for target management including creation, retrieval, editing, activation, deactivation, and deletion.
/// </summary>
public class TargetsService : ITargetsService
{
    #region Fields/Consts

    private readonly IMediator _mediator;
    private readonly ILogger<TargetsService> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetsService"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for sending commands and queries.</param>
    /// <param name="logger">The logger instance for logging information and errors.</param>
    public TargetsService(IMediator mediator, ILogger<TargetsService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    #region Methods

    /// <inheritdoc/>
    public async Task<IQueryable<TargetDto>> GetQueryableAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching GetQueryableTargetQuery");
        return await _mediator.Send(new GetQueryableTargetQuery(), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TargetDto>> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching GetTargetsQuery");
        return await _mediator.Send(new GetTargetsQuery(), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TargetDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching GetTargetByIdQuery for target {TargetId}", id);
        return await _mediator.Send(new GetTargetByIdQuery(id), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TargetDto> CreateAsync(CreateTargetDto createTargetDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching CreateTargetCommand");
        return await _mediator.Send(new CreateTargetCommand(createTargetDto), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TargetDto> EditAsync(Guid id, UpdateTargetDto updateTargetDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching UpdateTargetCommand for target {TargetId}", id);
        return await _mediator.Send(new UpdateTargetCommand(id, updateTargetDto), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TargetDto> ActivateAsync(Guid id, ActivateTargetDto activateTargetDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching ActivateTargetCommand for target {TargetId}", id);
        return await _mediator.Send(new ActivateTargetCommand(id, activateTargetDto), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TargetDto> DeactivateAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching DeactivateTargetCommand for target {TargetId}", id);
        return await _mediator.Send(new DeactivateTargetCommand(id), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Dispatching DeleteTargetCommand for target {TargetId}", id);
        return await _mediator.Send(new DeleteTargetCommand(id), cancellationToken);
    }

    #endregion
}
