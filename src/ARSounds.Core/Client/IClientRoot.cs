using ARSounds.ApplicationFlow;
using ARSounds.Core.ClaimsPrincipal;
using ARSounds.Core.Targets;

namespace ARSounds.Core.Client;

public interface IClientRoot : IDomainRoot
{
    IClaimsPrincipalState ClaimsPrincipalState { get; }

    ITargetsState TargetsState { get; }
}
