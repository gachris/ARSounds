using ARSounds.Application.Dtos;
using MediatR;

namespace ARSounds.Application.Commands;

public record SignInCommand : IRequest<RequestResultDto>;