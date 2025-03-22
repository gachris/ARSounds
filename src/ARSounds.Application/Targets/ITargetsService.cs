using ARSounds.Application.ImageRecognition.Response;
using ARSounds.Core.Targets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.ImageRecognition;

public interface ITargetsService
{
    Task<ResponseMessage<IEnumerable<Target>>> GetAsync(CancellationToken cancellationToken = default);
    Task<ResponseMessage<Target>> GetAsync(Guid id, CancellationToken cancellationToken = default);
}