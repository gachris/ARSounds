using ARSounds.Core;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.Auth.Commands;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, LogoutUserResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IApplicationEvents _applicationEvents;

    #endregion

    public LogoutUserCommandHandler(IAuthService authService, IApplicationEvents applicationEvents)
    {
        _authService = authService;
        _applicationEvents = applicationEvents;
    }

    public async Task<LogoutUserResultDto> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.LogoutAsync(cancellationToken);

            return new LogoutUserResultDto(true);
        }
        catch (Exception ex)
        {
            return new LogoutUserResultDto(false, ex.ToString());
        }
    }

    public async Task<LogoutUserResultDto> HandleInnerAsync(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authService.LogoutAsync(cancellationToken);

            return new LogoutUserResultDto(true);
        }
        catch (Exception ex)
        {
            return new LogoutUserResultDto(false, ex.ToString());
        }
    }
}
