using MediatR;

namespace ARSounds.Application.Auth.Commands;

public class SignInUserCommand : IRequest<SignInUserResultDto>
{
    public SignInUserCommand()
    {
    }
}