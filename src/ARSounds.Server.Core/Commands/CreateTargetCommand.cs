using ARSounds.Server.Core.Dtos;
using MediatR;

namespace ARSounds.Server.Core.Commands;

/// <summary>
/// Represents a command to create a new target.
/// </summary>
/// <param name="CreateTargetDto">The data transfer object containing the details needed to create the target.</param>
public record CreateTargetCommand(CreateTargetDto CreateTargetDto) : IRequest<TargetDto>;
