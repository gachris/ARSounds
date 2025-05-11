using ARSounds.ApiClient.Contracts;
using ARSounds.Application.Dtos;
using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.ClaimsPrincipal;
using ARSounds.Core.ClaimsPrincipal.Events;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ARSounds.Application.Commands;

/// <summary>
/// Handles the <see cref="SignInSilentCommand"/> by performing a silent authentication attempt.
/// If successful, it sets the authenticated user's claims into the application state and
/// raises related events.
/// </summary>
public class SignInSilentCommandHandler : IRequestHandler<SignInSilentCommand, RequestResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IClaimsPrincipalState _claimsPrincipalState;
    private readonly IMapper _mapper;
    private readonly IApplicationEvents _applicationEvents;
    private readonly ILogger<SignInSilentCommandHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SignInSilentCommandHandler"/> class.
    /// </summary>
    /// <param name="authService">Service to perform authentication logic.</param>
    /// <param name="claimsPrincipalState">State store for the authenticated user's identity.</param>
    /// <param name="mapper">AutoMapper instance to map DTOs to domain models.</param>
    /// <param name="applicationEvents">Event dispatcher for application lifecycle events.</param>
    /// <param name="logger">Logger for tracing and diagnostics.</param>
    public SignInSilentCommandHandler(
        IAuthService authService,
        IClaimsPrincipalState claimsPrincipalState,
        IMapper mapper,
        IApplicationEvents applicationEvents,
        ILogger<SignInSilentCommandHandler> logger)
    {
        _authService = authService;
        _claimsPrincipalState = claimsPrincipalState;
        _mapper = mapper;
        _applicationEvents = applicationEvents;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the silent sign-in command by attempting to authenticate the user in the background.
    /// If successful, user claims are extracted and stored in application state.
    /// </summary>
    /// <param name="request">The silent sign-in command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="RequestResultDto"/> representing the result of the operation.</returns>
    public async Task<RequestResultDto> Handle(SignInSilentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Silent sign-in process started.");
        _applicationEvents.Raise(new SignInStartedEvent());

        try
        {
            _logger.LogDebug("Calling auth service to perform silent sign-in.");
            await _authService.SignInSilentAsync(cancellationToken);

            if (_authService.IsAuthenticated)
            {
                _logger.LogInformation("User successfully authenticated via silent sign-in.");

                var userClaims = _mapper.Map<UserClaims>(_authService.UserClaims);

                _logger.LogDebug(
                    "User claims extracted: Id={Id}, Name={Name}, Role={Role}, Username={Username}, Email={Email}, EmailVerified={EmailVerified}",
                    userClaims.Id,
                    userClaims.Name,
                    userClaims.Role,
                    userClaims.Username,
                    userClaims.Email,
                    userClaims.EmailVerified);

                _claimsPrincipalState.SetUserClaims(userClaims);

                _logger.LogInformation("User claims set in application state.");
            }
            else
            {
                _logger.LogWarning("Silent sign-in failed. User is not authenticated.");
            }

            return new RequestResultDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during silent sign-in.");
            return new RequestResultDto("An error occurred during silent sign-in.", ex);
        }
        finally
        {
            _logger.LogInformation("Silent sign-in process finished.");
            _applicationEvents.Raise(new SignInFinishedEvent());
        }
    }

    #endregion
}
