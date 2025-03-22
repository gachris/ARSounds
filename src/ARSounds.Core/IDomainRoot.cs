namespace ARSounds.Core;

public interface IDomainRoot
{
    IReadOnlyList<ApplicationEvent> TakeApplicationEvents();
}
