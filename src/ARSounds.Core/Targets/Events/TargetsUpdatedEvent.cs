using ARSounds.ApplicationFlow;

namespace ARSounds.Core.Targets.Events;

public class TargetsUpdatedEvent : ApplicationEvent
{
    public TargetsUpdatedEvent(IReadOnlyCollection<Target> items) => Items = items;

    public IReadOnlyCollection<Target> Items { get; }
}