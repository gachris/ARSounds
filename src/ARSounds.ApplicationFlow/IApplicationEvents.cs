namespace ARSounds.ApplicationFlow;

public interface IApplicationEvents
{
    void Raise<T>(T domainEvent) where T : ApplicationEvent;

    void Register<T>(Action<T> eventHandler) where T : ApplicationEvent;

    void UnRegister<T>(Action<T> eventHandler) where T : ApplicationEvent;

    void UnRegister<T>(object obj);
}
