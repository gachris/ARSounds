using CommonServiceLocator;

namespace ARSounds.ApplicationFlow;

public class ApplicationEvents : IApplicationEvents
{
    #region Fields/Consts

    private readonly IServiceLocator _serviceLocator;
    private readonly SynchronizationContext _context;
    private readonly Dictionary<Type, List<Delegate>> _handlers = [];

    #endregion

    public ApplicationEvents(IServiceLocator serviceLocator, SynchronizationContext context)
    {
        _serviceLocator = serviceLocator;
        _context = context;
    }

    #region IApplicationEvents Implementation

    public void Raise<T>(T domainEvent) where T : ApplicationEvent
    {
        _context.Post(state => RaiseInternal(domainEvent), null);
    }

    public void Register<T>(Action<T> eventHandler) where T : ApplicationEvent
    {
        var type = typeof(T);

        if (!_handlers.ContainsKey(type))
        {
            _handlers[type] = [];
        }

        _handlers[type].Add(eventHandler);
    }

    public void UnRegister<T>(Action<T> eventHandler) where T : ApplicationEvent
    {
        var type = typeof(T);
        if (_handlers.ContainsKey(type) && _handlers[type].Contains(eventHandler))
        {
            _handlers[type].Remove(eventHandler);

            if (!_handlers[type].Any())
            {
                _handlers.Remove(type);
            }
        }
    }

    public void UnRegister<T>(object obj)
    {
        foreach (var handler in _handlers.ToList())
        {
            foreach (var v in handler.Value.ToList().Where(v => v.Target == obj))
            {
                handler.Value.Remove(v);
            }
        }
    }

    #endregion

    #region Methods

    private void RaiseInternal<T>(T domainEvent) where T : ApplicationEvent
    {
        var domainEventType = domainEvent.GetType();

        if (_handlers.ContainsKey(domainEventType))
        {
            foreach (var handler in _handlers[domainEventType])
            {
                handler.DynamicInvoke(domainEvent);
            }
        }

        var type = typeof(IApplicationEventHandler<>).MakeGenericType(domainEventType);
        var handlers = _serviceLocator.GetAllInstances(type).ToList<dynamic?>();
        handlers.ForEach(e =>
        {
            e?.Handle((dynamic)domainEvent);
        });
    }

    #endregion
}