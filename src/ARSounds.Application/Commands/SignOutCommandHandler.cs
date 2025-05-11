using ARSounds.ApiClient.Contracts;
using ARSounds.Application.Dtos;
using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.ClaimsPrincipal;
using ARSounds.Core.ClaimsPrincipal.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ARSounds.Application.Commands;

/// <summary>
/// Handles the <see cref="SignOutCommand"/> to perform user sign-out operations.
/// This includes calling the auth service, clearing the user's state, and raising related application events.
/// </summary>
public class SignOutCommandHandler : IRequestHandler<SignOutCommand, RequestResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IClaimsPrincipalState _claimsPrincipalState;
    private readonly IApplicationEvents _applicationEvents;
    private readonly ILogger<SignOutCommandHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SignOutCommandHandler"/> class.
    /// </summary>
    /// <param name="authService">The authentication service responsible for signing out.</param>
    /// <param name="claimsPrincipalState">The state store managing the authenticated user's claims.</param>
    /// <param name="applicationEvents">The event dispatcher for application lifecycle events.</param>
    /// <param name="logger">Logger instance for diagnostics and auditing.</param>
    public SignOutCommandHandler(
        IAuthService authService,
        IClaimsPrincipalState claimsPrincipalState,
        IApplicationEvents applicationEvents,
        ILogger<SignOutCommandHandler> logger)
    {
        _authService = authService;
        _claimsPrincipalState = claimsPrincipalState;
        _applicationEvents = applicationEvents;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the <see cref="SignOutCommand"/> by signing the user out,
    /// clearing their identity from application state, and raising sign-out lifecycle events.
    /// </summary>
    /// <param name="request">The sign-out command request.</param>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    /// <returns>A <see cref="RequestResultDto"/> indicating the result of the sign-out operation.</returns>
    public async Task<RequestResultDto> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sign-out process started.");
        _applicationEvents.Raise(new SignOutStartedEvent());

        try
        {
            _logger.LogDebug("Calling auth service to sign out.");
            await _authService.SignOutAsync(cancellationToken);

            _logger.LogDebug("Clearing user from claims principal state.");
            _claimsPrincipalState.ClearUserClaims();

            _logger.LogInformation("User successfully signed out.");
            return new RequestResultDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the sign-out process.");
            return new RequestResultDto("An error occurred during sign-out.", ex);
        }
        finally
        {
            _logger.LogInformation("Sign-out process finished.");
            _applicationEvents.Raise(new SignOutFinishedEvent());
        }
    }

    #endregion
}
