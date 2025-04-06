using ARSounds.Server.Core.Dtos;
using MediatR;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Represents a command to activate a target.
/// </summary>
/// <param name="TargetId">The unique identifier of the target to activate.</param>
/// <param name="ActivateTargetDto">The data transfer object containing the activation details for the target.</param>
public record ActivateTargetCommand(Guid TargetId, ActivateTargetDto ActivateTargetDto) : IRequest<TargetDto>;
