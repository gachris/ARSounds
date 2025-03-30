using ARSounds.ApiClient.Dtos;
using ARSounds.ApiClient.Response;

namespace ARSounds.ApiClient.Contracts;

public interface ITargetsService
{
    Task<ResponseMessage<IEnumerable<TargetDto>>> GetAsync(string bearerToken, CancellationToken cancellationToken = default);

    Task<ResponseMessage<TargetDto>> GetAsync(Guid id, string bearerToken, CancellationToken cancellationToken = default);
}