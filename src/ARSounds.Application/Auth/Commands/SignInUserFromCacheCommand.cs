using MediatR;

namespace ARSounds.Application.Auth.Commands;

public class SignInUserFromCacheCommand : IRequest<SignInUserResultDto>
{
}