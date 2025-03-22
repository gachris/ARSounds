using ARSounds.Core;
using ARSounds.Core.Auth.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.Auth.Commands;

public class SignInUserFromCacheCommandHandler : IRequestHandler<SignInUserFromCacheCommand, SignInUserResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IApplicationEvents _applicationEvents;

    #endregion

    public SignInUserFromCacheCommandHandler(IAuthService authService, IApplicationEvents applicationEvents)
    {
        _authService = authService;
        _applicationEvents = applicationEvents;
    }

    public async Task<SignInUserResultDto> Handle(SignInUserFromCacheCommand request, CancellationToken cancellationToken)
    {
        _applicationEvents.Raise(new SignInStartedEvent());

        var unit = await HandleInnerAsync(request, cancellationToken);

        _applicationEvents.Raise(new SignInFinishedEvent());

        return unit;
    }

    public async Task<SignInUserResultDto> HandleInnerAsync(SignInUserFromCacheCommand request, CancellationToken cancellationToken)
    {
        await _authService.TryLoginFromCacheAsync(cancellationToken);
        return new SignInUserResultDto(_authService.IsUserLoggedIn);
    }
}
