using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.ClaimsPrincipal;
using ARSounds.Core.Targets;

namespace ARSounds.Core.Client;

public interface IClientRoot : IAggregateRoot
{
    IClaimsPrincipalState ClaimsPrincipalState { get; }

    ITargetsState TargetsState { get; }
}
