using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Commands;

/// <summary>
/// Represents a command to initiate the user sign-out process.
/// </summary>
/// <remarks>
/// This command is handled by <see cref="SignOutCommandHandler"/> and is used
/// to clear the user's authentication state and perform necessary cleanup.
/// </remarks>
public record SignOutCommand : IRequest<RequestResultDto>;