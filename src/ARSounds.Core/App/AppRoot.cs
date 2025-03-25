using ARSounds.ApplicationFlow;
using ARSounds.Core.App.Events;
using ARSounds.Core.Targets;

namespace ARSounds.Core.App;

public class AppRoot : AggregateRoot, IAppRoot
{
    #region Fields/Consts

    private ITargetsComponent _targets = default!;

    #endregion

    #region Properties

    protected override IEnumerable<AggregateRoot> ChildAggregates =>
        new[]
        {
            (AggregateRoot)Targets
        };

    #endregion

    public AppRoot() => (Targets) = (new TargetsComponent());

    public ITargetsComponent Targets
    {
        get => _targets;
        private set
        {
            _targets = value;
            _targets.ClearEvents();
            AddEvent(new TargetsStateSetEvent());
        }
    }
}
