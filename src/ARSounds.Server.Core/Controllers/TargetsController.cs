using ARSounds.Server.Core.Auth;
using ARSounds.Server.Core.Contracts;
using ARSounds.Server.Core.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Controllers;

/// <summary>
/// Controller for managing target operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationConsts.BearerPolicy)]
public class TargetsController : ControllerBase
{
    #region Fields/Consts

    private readonly ITargetsService _targetsService;
    private readonly ILogger<TargetsController> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetsController"/> class.
    /// </summary>
    /// <param name="targetsService">The target service instance.</param>
    /// <param name="logger">The logger instance.</param>
    public TargetsController(ITargetsService targetsService, ILogger<TargetsController> logger)
    {
        _targetsService = targetsService;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Retrieves a paginated list of targets based on the specified query.
    /// </summary>
    /// <param name="query">The query parameters for browsing targets.</param>
    /// <returns>An <see cref="IActionResult"/> containing the paged response of targets.</returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get([FromQuery] TargetBrowserQuery query)
    {
        _logger.LogInformation("Received request to get targets list with query: {@Query}", query);
        var pagedResponse = await _targetsService.GetAsync(query, CancellationToken.None);
        _logger.LogInformation("Returning paged targets list with total records: {TotalRecords}", pagedResponse.TotalRecords);
        return new OkObjectResult(pagedResponse);
    }

    /// <summary>
    /// Retrieves a specific target by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the target.</param>
    /// <returns>An <see cref="IActionResult"/> containing the target details.</returns>
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        _logger.LogInformation("Received request to get target with id: {Id}", id);
        var responseMessage = await _targetsService.GetAsync(id, CancellationToken.None);
        _logger.LogInformation("Returning target details for id: {Id}", id);
        return new OkObjectResult(responseMessage);
    }

    /// <summary>
    /// Creates a new target.
    /// </summary>
    /// <param name="body">The request body containing target creation details.</param>
    /// <returns>An <see cref="IActionResult"/> with the created target information and location header.</returns>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] CreateTargetRequest body)
    {
        _logger.LogInformation("Received request to create a new target with description: {Description}", body.Description);
        var responseMessage = await _targetsService.CreateAsync(body, CancellationToken.None);
        var url = Url.Action("Get", new { id = responseMessage.Response.Result });

        ArgumentException.ThrowIfNullOrEmpty(url, nameof(url));

        _logger.LogInformation("Target created with id: {TargetId}. Location: {Url}", responseMessage.Response.Result, url);

        var locationUri = new Uri(url, UriKind.Relative);
        return new CreatedResult(locationUri, responseMessage);
    }

    /// <summary>
    /// Updates an existing target.
    /// </summary>
    /// <param name="id">The unique identifier of the target to update.</param>
    /// <param name="body">The request body containing updated target details.</param>
    /// <returns>An <see cref="IActionResult"/> with the update operation result.</returns>
    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, [FromBody] UpdateTargetRequest body)
    {
        _logger.LogInformation("Received request to edit target with id: {Id}", id);
        var responseMessage = await _targetsService.EditAsync(id, body, CancellationToken.None);
        _logger.LogInformation("Target with id: {Id} updated successfully.", id);
        return new OkObjectResult(responseMessage);
    }

    /// <summary>
    /// Activates a target.
    /// </summary>
    /// <param name="id">The unique identifier of the target to activate.</param>
    /// <param name="body">The request body containing activation details.</param>
    /// <returns>An <see cref="IActionResult"/> with the activation operation result.</returns>
    [HttpPost]
    [Route("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, [FromBody] ActivateTargetRequest body)
    {
        _logger.LogInformation("Received request to activate target with id: {Id}", id);
        var responseMessage = await _targetsService.ActivateAsync(id, body, CancellationToken.None);
        _logger.LogInformation("Target with id: {Id} activated successfully.", id);
        return new OkObjectResult(responseMessage);
    }

    /// <summary>
    /// Deactivates a target.
    /// </summary>
    /// <param name="id">The unique identifier of the target to deactivate.</param>
    /// <returns>An <see cref="IActionResult"/> with the deactivation operation result.</returns>
    [HttpPost]
    [Route("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        _logger.LogInformation("Received request to deactivate target with id: {Id}", id);
        var responseMessage = await _targetsService.DeactivateAsync(id, CancellationToken.None);
        _logger.LogInformation("Target with id: {Id} deactivated successfully.", id);
        return new OkObjectResult(responseMessage);
    }

    /// <summary>
    /// Deletes a target.
    /// </summary>
    /// <param name="id">The unique identifier of the target to delete.</param>
    /// <returns>An <see cref="IActionResult"/> with the deletion operation result.</returns>
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Received request to delete target with id: {Id}", id);
        var responseMessage = await _targetsService.DeleteAsync(id, CancellationToken.None);
        _logger.LogInformation("Target with id: {Id} deleted successfully.", id);
        return new OkObjectResult(responseMessage);
    }

    #endregion
}
