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
/// Handles the <see cref="SignInCommand"/> to initiate a user sign-in process.
/// Responsible for calling the auth service, setting the authenticated user in state,
/// and raising application events.
/// </summary>
public class SignInCommandHandler : IRequestHandler<SignInCommand, RequestResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly IClaimsPrincipalState _claimsPrincipalState;
    private readonly IMapper _mapper;
    private readonly IApplicationEvents _applicationEvents;
    private readonly ILogger<SignInCommandHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SignInCommandHandler"/> class.
    /// </summary>
    /// <param name="authService">The authentication service used to perform sign-in.</param>
    /// <param name="claimsPrincipalState">The state object to hold the authenticated user.</param>
    /// <param name="mapper">AutoMapper instance to map DTOs to domain models.</param>
    /// <param name="applicationEvents">The application event dispatcher.</param>
    /// <param name="logger">Logger instance for tracing the sign-in process.</param>
    public SignInCommandHandler(
        IAuthService authService,
        IClaimsPrincipalState claimsPrincipalState,
        IMapper mapper,
        IApplicationEvents applicationEvents,
        ILogger<SignInCommandHandler> logger)
    {
        _authService = authService;
        _claimsPrincipalState = claimsPrincipalState;
        _mapper = mapper;
        _applicationEvents = applicationEvents;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the execution of the <see cref="SignInCommand"/>.
    /// Performs user sign-in via the authentication service and updates user state.
    /// </summary>
    /// <param name="request">The sign-in command request.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>A <see cref="RequestResultDto"/> indicating the outcome of the operation.</returns>
    public async Task<RequestResultDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sign-in process started.");
        _applicationEvents.Raise(new SignInStartedEvent());

        try
        {
            _logger.LogDebug("Calling auth service to sign in user.");
            await _authService.SignInAsync(cancellationToken);

            if (_authService.IsAuthenticated)
            {
                _logger.LogInformation("User successfully authenticated.");

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
                _logger.LogWarning("Authentication failed. User is not authenticated.");
            }

            return new RequestResultDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the sign-in process.");
            return new RequestResultDto("An error occurred during sign-in.", ex);
        }
        finally
        {
            _logger.LogInformation("Sign-in process finished.");
            _applicationEvents.Raise(new SignInFinishedEvent());
        }
    }

    #endregion
}