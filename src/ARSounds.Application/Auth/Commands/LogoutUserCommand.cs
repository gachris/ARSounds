using MediatR;

namespace ARSounds.Application.Auth.Commands;

public class LogoutUserCommand : IRequest<LogoutUserResultDto>
{
}