using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.ClaimsPrincipal;
using ARSounds.Core.Client.Events;
using ARSounds.Core.Targets;

namespace ARSounds.Core.Client;

public class ClientRoot : AggregateRoot, IClientRoot
{
    #region Fields/Consts

    private ITargetsState _targetsState = null!;
    private IClaimsPrincipalState _claimsPrincipalState = null!;

    #endregion

    #region Properties

    protected override IEnumerable<AggregateRoot> ChildAggregates =>
    [
        (AggregateRoot)TargetsState,
        (AggregateRoot)ClaimsPrincipalState
    ];

    public ITargetsState TargetsState
    {
        get => _targetsState;
        private set
        {
            _targetsState = value;
            _targetsState.ClearEvents();
            AddEvent(new TargetsStateSetEvent());
        }
    }

    public IClaimsPrincipalState ClaimsPrincipalState
    {
        get => _claimsPrincipalState;
        private set
        {
            _claimsPrincipalState = value;
            _claimsPrincipalState.ClearEvents();
            AddEvent(new TargetsStateSetEvent());
        }
    }

    #endregion

    public ClientRoot()
    {
        TargetsState = new TargetsState();
        ClaimsPrincipalState = new ClaimsPrincipalState();
    }
}
