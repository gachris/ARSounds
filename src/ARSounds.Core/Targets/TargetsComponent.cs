using ARSounds.ApplicationFlow;
using ARSounds.Core.Targets.Events;

namespace ARSounds.Core.Targets;

public class TargetsComponent : AggregateRoot, ITargetsComponent
{
    #region Fields/Consts

    private readonly List<Target> _targets;

    #endregion

    #region Properties

    public IReadOnlyList<Target> Targets => _targets;

    #endregion

    public TargetsComponent()
    {
        _targets = new List<Target>();
    }

    public void SetTargetsResult(IEnumerable<Target> contacts)
    {
        _targets.Clear();
        _targets.AddRange(contacts);
        AddEvent(new TargetsUpdatedEvent(_targets));
    }
}
