using ARSounds.Application.Dtos;
using ARSounds.Application.Services;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using MediatR;

namespace ARSounds.Application.Commands;

public class SignInSilentCommandHandler : IRequestHandler<SignInSilentCommand, RequestResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IApplicationEvents _applicationEvents;

    #endregion

    public SignInSilentCommandHandler(IAuthService authService, IApplicationEvents applicationEvents)
    {
        _authService = authService;
        _applicationEvents = applicationEvents;
    }

    public async Task<RequestResultDto> Handle(SignInSilentCommand request, CancellationToken cancellationToken)
    {
        _applicationEvents.Raise(new SignInStartedEvent());

        var unit = await HandleInnerAsync(request, cancellationToken);

        _applicationEvents.Raise(new SignInFinishedEvent());

        return unit;
    }

    public async Task<RequestResultDto> HandleInnerAsync(SignInSilentCommand request, CancellationToken cancellationToken)
    {
        await _authService.SignInSilentAsync(cancellationToken);
        return new RequestResultDto();
    }
}
