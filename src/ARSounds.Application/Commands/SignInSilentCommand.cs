using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Commands;

/// <summary>
/// Represents a command to perform a silent sign-in process,
/// typically used when re-authenticating an existing user without user interaction.
/// </summary>
/// <remarks>
/// This command is handled by <see cref="SignInSilentCommandHandler"/>.
/// </remarks>
public record SignInSilentCommand : IRequest<RequestResultDto>;