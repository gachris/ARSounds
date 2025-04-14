namespace ARSounds.ApplicationFlow;

public interface IApplicationEventHandler<in T> where T : ApplicationEvent
{
    void Handle(T e);
}
