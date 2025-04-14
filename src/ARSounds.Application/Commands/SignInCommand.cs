using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Commands;

/// <summary>
/// Represents a command to initiate the sign-in process for a user.
/// </summary>
/// <remarks>
/// This command triggers authentication via the appropriate auth service,
/// and should be handled by <see cref="SignInCommandHandler"/>.
/// </remarks>
public record SignInCommand : IRequest<RequestResultDto>;