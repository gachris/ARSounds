using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.Auth.Commands;

public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, SignInUserResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IApplicationEvents _applicationEvents;

    #endregion

    public SignInUserCommandHandler(IAuthService authService, IApplicationEvents applicationEvents)
    {
        _authService = authService;
        _applicationEvents = applicationEvents;
    }

    public async Task<SignInUserResultDto> Handle(SignInUserCommand request, CancellationToken cancellationToken)
    {
        _applicationEvents.Raise(new SignInStartedEvent());

        var signInUserResultDto = await HandleInnerAsync(request, cancellationToken);

        _applicationEvents.Raise(new SignInFinishedEvent());

        return signInUserResultDto;
    }

    public async Task<SignInUserResultDto> HandleInnerAsync(SignInUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.LoginAsync(cancellationToken);

            return new SignInUserResultDto(true);
        }
        catch (Exception ex)
        {
            return new SignInUserResultDto(false, ex.ToString());
        }
    }
}
