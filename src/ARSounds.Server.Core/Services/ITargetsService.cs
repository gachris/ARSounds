using ARSounds.Server.Core.Filters;
using ARSounds.Server.Core.Requests;
using OpenVision.Shared.Responses;
using TargetResponse = ARSounds.Server.Core.Responses.TargetResponse;

namespace ARSounds.Server.Core.Services;

public interface ITargetsService
{
    Task<IPagedResponse<IEnumerable<TargetResponse>>> GetAsync(TargetBrowserQuery query, CancellationToken cancellationToken);

    Task<IResponseMessage<TargetResponse>> Get(Guid id, CancellationToken cancellationToken);

    Task<IResponseMessage<Guid>> Create(CreateTargetRequest body, CancellationToken cancellationToken);

    Task<IResponseMessage> Edit(Guid id, UpdateTargetRequest body, CancellationToken cancellationToken);

    Task<IResponseMessage> Activate(Guid id, ActivateTargetRequest body, CancellationToken cancellationToken);

    Task<IResponseMessage> Deactivate(Guid id, CancellationToken cancellationToken);

    Task<IResponseMessage> Delete(Guid id, CancellationToken cancellationToken);
}