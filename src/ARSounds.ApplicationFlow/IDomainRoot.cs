namespace ARSounds.ApplicationFlow;

public interface IDomainRoot
{
    IReadOnlyList<ApplicationEvent> TakeApplicationEvents();
}
