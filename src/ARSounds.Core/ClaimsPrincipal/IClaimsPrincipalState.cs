namespace ARSounds.Core.ClaimsPrincipal;

public interface IClaimsPrincipalState
{
    UserClaims? UserClaims { get; }

    void ClearEvents();

    void SetUserClaims(UserClaims userClaims);

    void ClearUserClaims();
}
