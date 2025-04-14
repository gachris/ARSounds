using ARSounds.Server.Core.Dtos;
using MediatR;

namespace ARSounds.Server.Core.Queries;

/// <summary>
/// Represents a query to retrieve all targets.
/// </summary>
public record GetTargetsQuery() : IRequest<IEnumerable<TargetDto>>;