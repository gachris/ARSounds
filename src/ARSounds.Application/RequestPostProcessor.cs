using ARSounds.ApplicationFlow;
using MediatR;
using MediatR.Pipeline;

namespace ARSounds.Application;

/// <summary>
/// A MediatR post-processor that dispatches application-level events
/// after a request has been handled.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class RequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IApplicationEventsDispatcher _dispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestPostProcessor{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="dispatcher">The application events dispatcher to invoke after the request is processed.</param>
    public RequestPostProcessor(IApplicationEventsDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    /// <summary>
    /// Processes the request after it has been handled by the main handler.
    /// This implementation dispatches queued application events.
    /// </summary>
    /// <param name="request">The original request.</param>
    /// <param name="response">The response returned from the handler.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        _dispatcher.Dispatch();
        return Unit.Task;
    }
}