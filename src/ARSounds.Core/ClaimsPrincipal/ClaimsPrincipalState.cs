using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.ClaimsPrincipal.Events;

namespace ARSounds.Core.ClaimsPrincipal;

public class ClaimsPrincipalState : AggregateRoot, IClaimsPrincipalState
{
    #region Fields/Consts

    #endregion

    #region Properties

    public UserClaims? UserClaims { get; private set; }

    #endregion

    #region Methods

    public void SetUserClaims(UserClaims userClaims)
    {
        UserClaims = userClaims;
        AddEvent(new SignedInEvent());
    }

    public void ClearUserClaims()
    {
        UserClaims = null;
        AddEvent(new SignedOutEvent());
    }

    #endregion
}