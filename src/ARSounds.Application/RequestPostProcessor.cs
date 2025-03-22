using ARSounds.Core;
using MediatR;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application;

public class RequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IApplicationEventsDispatcher _dispatcher;

    public RequestPostProcessor(IApplicationEventsDispatcher dispatcher) => _dispatcher = dispatcher;

    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        _dispatcher.Dispatch();
        return Unit.Task;
    }
}
