using DevToolbox.Core.ApplicationFlow;

namespace ARSounds.Core.Targets.Events;

public class TargetsCollectionUpdatedEvent : ApplicationEvent
{
    public IReadOnlyCollection<Target> Items { get; }

    public TargetsCollectionUpdatedEvent(IReadOnlyCollection<Target> items)
    {
        Items = items;
    }
}