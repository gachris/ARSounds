using ARSounds.ApiClient.Contracts;
using ARSounds.ApiClient.Response;
using ARSounds.Application.Dtos;
using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.Targets;
using ARSounds.Core.Targets.Events;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ARSounds.Application.Queries;

/// <summary>
/// Handles the <see cref="RetrieveTargetsQuery"/> by retrieving target data from the API,
/// mapping it to domain models, updating the application state, and raising related events.
/// </summary>
public class RetrieveTargetsQueryHandler : IRequestHandler<RetrieveTargetsQuery, RequestResultDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;
    private readonly ITargetsService _targetsService;
    private readonly IApplicationEvents _applicationEvents;
    private readonly IMapper _mapper;
    private readonly ITargetsState _targetsState;
    private readonly ILogger<RetrieveTargetsQueryHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="RetrieveTargetsQueryHandler"/> class.
    /// </summary>
    /// <param name="authService">Authentication service to check user authorization.</param>
    /// <param name="targetsService">Service to retrieve target data from the API.</param>
    /// <param name="targetsState">State container for storing retrieved targets.</param>
    /// <param name="mapper">AutoMapper instance to map DTOs to domain models.</param>
    /// <param name="applicationEvents">Dispatcher for application lifecycle events.</param>
    /// <param name="logger">Logger instance for tracing and diagnostics.</param>
    public RetrieveTargetsQueryHandler(
        IAuthService authService,
        ITargetsService targetsService,
        ITargetsState targetsState,
        IMapper mapper,
        IApplicationEvents applicationEvents,
        ILogger<RetrieveTargetsQueryHandler> logger)
    {
        _authService = authService;
        _targetsService = targetsService;
        _targetsState = targetsState;
        _mapper = mapper;
        _applicationEvents = applicationEvents;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Handles the execution of the <see cref="RetrieveTargetsQuery"/> by calling the API,
    /// validating the response, mapping the results, and updating the state.
    /// </summary>
    /// <param name="request">The query object representing the request for targets.</param>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    /// <returns>A <see cref="RequestResultDto"/> representing the result of the operation.</returns>
    public async Task<RequestResultDto> Handle(RetrieveTargetsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting retrieval of targets...");
            _applicationEvents.Raise(new RetrieveTargetsStartedEvent());

            if (_authService.Token is null || !_authService.IsAuthenticated)
            {
                _logger.LogWarning("Attempted to retrieve targets without valid authentication.");
                throw new InvalidOperationException("Authorization token is missing or invalid.");
            }

            _logger.LogDebug("Fetching targets from API using token: {Token}", _authService.Token.AccessToken);

            var responseMessage = await _targetsService.GetAsync(_authService.Token.AccessToken, cancellationToken);

            if (responseMessage.StatusCode == StatusCode.Success)
            {
                var targets = responseMessage.Response?.Result;

                if (targets is null)
                {
                    _logger.LogError("API response was successful but returned null targets.");
                    throw new InvalidOperationException("Targets response payload was null.");
                }

                var mappedTargets = _mapper.Map<IEnumerable<Target>>(targets);
                _targetsState.SetTargetsResult(mappedTargets);

                _logger.LogInformation("Successfully retrieved and mapped {Count} targets.", mappedTargets.Count());
            }
            else
            {
                //var errorMessage = !string.IsNullOrWhiteSpace(responseMessage.Message)
                //    ? responseMessage.Message
                //    : "Unknown error occurred while retrieving targets.";

                //_logger.LogError("API returned error status. Code: {Code}, Message: {Message}",
                //    responseMessage.StatusCode, responseMessage.Message);

                //throw new InvalidOperationException($"API Error: {errorMessage}");

                throw new InvalidOperationException("Unable to retrieve targets.");
            }

            return new RequestResultDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving targets.");
            return new RequestResultDto("Unable to retrieve targets.", ex);
        }
        finally
        {
            _logger.LogInformation("Finished retrieval process for targets.");
            _applicationEvents.Raise(new RetrieveTargetsFinishedEvent());
        }
    }

    #endregion
}