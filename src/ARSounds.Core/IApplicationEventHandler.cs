namespace ARSounds.Core;

public interface IApplicationEventHandler<in T> where T : ApplicationEvent
{
    void Handle(T e);
}
