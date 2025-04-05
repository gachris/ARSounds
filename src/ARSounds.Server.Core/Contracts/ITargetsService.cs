using ARSounds.Server.Core.Requests;
using OpenVision.Shared.Responses;
using TargetResponse = ARSounds.Server.Core.Responses.TargetResponse;

namespace ARSounds.Server.Core.Contracts;

public interface ITargetsService
{
    Task<IPagedResponse<IEnumerable<TargetResponse>>> GetAsync(TargetBrowserQuery query, CancellationToken cancellationToken);

    Task<IResponseMessage<TargetResponse>> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<IResponseMessage<Guid>> CreateAsync(CreateTargetRequest body, CancellationToken cancellationToken);

    Task<IResponseMessage> EditAsync(Guid id, UpdateTargetRequest body, CancellationToken cancellationToken);

    Task<IResponseMessage> ActivateAsync(Guid id, ActivateTargetRequest body, CancellationToken cancellationToken);

    Task<IResponseMessage> DeactivateAsync(Guid id, CancellationToken cancellationToken);

    Task<IResponseMessage> DeleteAsync(Guid id, CancellationToken cancellationToken);
}