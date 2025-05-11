using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.Targets.Events;

namespace ARSounds.Core.Targets;

public class TargetsState : AggregateRoot, ITargetsState
{
    #region Fields/Consts

    private readonly List<Target> _targets = [];

    #endregion

    #region Properties

    public IReadOnlyList<Target> Targets => _targets;

    #endregion

    #region Methods

    public void SetTargetsResult(IEnumerable<Target> targets)
    {
        _targets.Clear();
        _targets.AddRange(targets);
        AddEvent(new TargetsCollectionUpdatedEvent(_targets));
    }

    #endregion
}
