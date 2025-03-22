using ARSounds.Web.Api.Core.Auth;
using ARSounds.Web.Api.Core.Services;
using ARSounds.Web.Core.Filters;
using ARSounds.Web.Core.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARSounds.Web.Api.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationConsts.BearerPolicy)]
public class TargetsController : ControllerBase
{
    private readonly ITargetsService _targetsService;

    public TargetsController(ITargetsService targetsService)
    {
        _targetsService = targetsService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Get([FromQuery] TargetBrowserQuery query)
    {
        var pagedResponse = await _targetsService.GetAsync(query, CancellationToken.None);
        return new OkObjectResult(pagedResponse);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var responseMessage = await _targetsService.Get(id, CancellationToken.None);
        return new OkObjectResult(responseMessage);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] CreateTargetRequest body)
    {
        var responseMessage = await _targetsService.Create(body, CancellationToken.None);

        // Create the location URI for the new resource
        var locationUri = new Uri(Url.Action("Get", new { id = responseMessage.Response.Result }), UriKind.Relative);

        // Return the CreatedResult with the location URI
        return new CreatedResult(locationUri, responseMessage);
    }

    [HttpPost]
    [Route("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, [FromBody] UpdateTargetRequest body)
    {
        var responseMessage = await _targetsService.Edit(id, body, CancellationToken.None);
        return new OkObjectResult(responseMessage);
    }

    [HttpPost]
    [Route("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, [FromBody] ActivateTargetRequest body)
    {
        var responseMessage = await _targetsService.Activate(id, body, CancellationToken.None);
        return new OkObjectResult(responseMessage);
    }

    [HttpPost]
    [Route("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var responseMessage = await _targetsService.Deactivate(id, CancellationToken.None);
        return new OkObjectResult(responseMessage);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var responseMessage = await _targetsService.Delete(id, CancellationToken.None);
        return new OkObjectResult(responseMessage);
    }
}
