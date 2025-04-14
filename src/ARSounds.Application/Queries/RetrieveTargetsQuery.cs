using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Queries;

/// <summary>
/// Represents a query to retrieve a list of available targets from the backend API.
/// </summary>
/// <remarks>
/// This query is handled by <see cref="RetrieveTargetsQueryHandler"/>.
/// </remarks>
public record RetrieveTargetsQuery : IRequest<RequestResultDto>;