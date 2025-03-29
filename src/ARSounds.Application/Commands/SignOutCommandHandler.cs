using ARSounds.Application.Dtos;
using ARSounds.Application.Services;
using ARSounds.ApplicationFlow;
using MediatR;

namespace ARSounds.Application.Commands;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, RequestResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IApplicationEvents _applicationEvents;

    #endregion

    public SignOutCommandHandler(IAuthService authService, IApplicationEvents applicationEvents)
    {
        _authService = authService;
        _applicationEvents = applicationEvents;
    }

    public async Task<RequestResultDto> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.SignOutAsync(cancellationToken);

            return new RequestResultDto();
        }
        catch (Exception ex)
        {
            return new RequestResultDto(ex.ToString(), ex);
        }
    }
}