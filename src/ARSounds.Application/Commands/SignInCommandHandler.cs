using ARSounds.Application.Dtos;
using ARSounds.Application.Services;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using MediatR;

namespace ARSounds.Application.Commands;

public class SignInCommandHandler : IRequestHandler<SignInCommand, RequestResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IApplicationEvents _applicationEvents;

    #endregion

    public SignInCommandHandler(IAuthService authService, IApplicationEvents applicationEvents)
    {
        _authService = authService;
        _applicationEvents = applicationEvents;
    }

    public async Task<RequestResultDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        _applicationEvents.Raise(new SignInStartedEvent());

        var signInUserResultDto = await HandleInnerAsync(request, cancellationToken);

        _applicationEvents.Raise(new SignInFinishedEvent());

        return signInUserResultDto;
    }

    public async Task<RequestResultDto> HandleInnerAsync(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.SignInAsync(cancellationToken);

            return new RequestResultDto();
        }
        catch (Exception ex)
        {
            return new RequestResultDto(ex.ToString(), ex);
        }
    }
}
