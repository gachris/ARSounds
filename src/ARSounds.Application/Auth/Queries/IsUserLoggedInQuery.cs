using MediatR;

namespace ARSounds.Application.Auth.Queries;

public class IsUserLoggedInQuery : IRequest<bool>
{
}