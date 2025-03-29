using ARSounds.Application.Response;
using ARSounds.Core.Targets;

namespace ARSounds.Application.Services;

public interface ITargetsService
{
    Task<ResponseMessage<IEnumerable<Target>>> GetAsync(CancellationToken cancellationToken = default);

    Task<ResponseMessage<Target>> GetAsync(Guid id, CancellationToken cancellationToken = default);
}