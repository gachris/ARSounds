using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Commands;

public record SignOutCommand : IRequest<RequestResultDto>;