using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.Auth.Queries;

public class IsUserLoggedInQueryHandler : IRequestHandler<IsUserLoggedInQuery, bool>
{
    #region Fields/Consts

    private readonly IAuthService _authService;

    #endregion

    public IsUserLoggedInQueryHandler(IAuthService authService) => _authService = authService;

    public Task<bool> Handle(IsUserLoggedInQuery request, CancellationToken cancellationToken) => Task.FromResult(_authService.IsUserLoggedIn);
}
