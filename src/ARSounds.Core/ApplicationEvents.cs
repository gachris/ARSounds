using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ARSounds.Core;

public class ApplicationEvents : IApplicationEvents
{
    #region Fields/Consts

    private readonly IServiceProvider _serviceProvider;
    private readonly SynchronizationContext _context;
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();

    #endregion

    public ApplicationEvents(IServiceProvider serviceProvider, SynchronizationContext context)
    {
        _serviceProvider = serviceProvider;
        _context = context;
    }

    #region IApplicationEvents Implementation

    public void Raise<T>(T domainEvent) where T : ApplicationEvent => _context.Post(state => RaiseInternal(domainEvent), null);

    public void Register<T>(Action<T> eventHandler) where T : ApplicationEvent
    {
        var type = typeof(T);
        if (!_handlers.ContainsKey(type)) _handlers[type] = new List<Delegate>();
        _handlers[type].Add(eventHandler);
    }

    public void UnRegister<T>(Action<T> eventHandler) where T : ApplicationEvent
    {
        var type = typeof(T);
        if (!_handlers.ContainsKey(type) && _handlers[type].Contains(eventHandler))
        {
            _handlers[type].Remove(eventHandler);

            if (!_handlers[type].Any()) _handlers.Remove(type);
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

        Debug.WriteLine($"EVENT => {domainEventType.Name} {JsonConvert.SerializeObject(domainEvent)}");

        if (_handlers.ContainsKey(domainEventType))
        {
            foreach (var handler in _handlers[domainEventType])
            {
                Debug.WriteLine($"HANDLER => For {domainEventType.Name} on {handler.Target?.ToString()?.Split('.').Last()}");
                handler.DynamicInvoke(domainEvent);
            }
        }

        var type = typeof(IApplicationEventHandler<>).MakeGenericType(domainEventType);
        var handlers = _serviceProvider.GetServices(type).ToList<dynamic>();
        handlers.ForEach(e =>
        {
            if (e is not null)
            {
                Debug.WriteLine($"HANDLER => For {domainEventType.Name} on {e.GetType().Name}");
                e.Handle((dynamic)domainEvent);
            }
        });

    }

    #endregion
}
