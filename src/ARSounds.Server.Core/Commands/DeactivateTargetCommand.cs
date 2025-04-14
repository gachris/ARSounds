using ARSounds.Server.Core.Dtos;
using MediatR;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Represents a command to deactivate a target.
/// </summary>
/// <param name="TargetId">The unique identifier of the target to deactivate.</param>
public record DeactivateTargetCommand(Guid TargetId) : IRequest<TargetDto>;