using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Commands;

public record SignInSilentCommand : IRequest<RequestResultDto>;