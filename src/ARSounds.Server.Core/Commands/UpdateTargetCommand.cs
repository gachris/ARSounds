using ARSounds.Server.Core.Dtos;
using MediatR;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Represents a command to update an existing target.
/// </summary>
/// <param name="TargetId">The unique identifier of the target to update.</param>
/// <param name="UpdateTargetDto">The data transfer object containing the updated target details.</param>
public record UpdateTargetCommand(Guid TargetId, UpdateTargetDto UpdateTargetDto) : IRequest<TargetDto>;