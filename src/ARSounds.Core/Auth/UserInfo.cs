using System.Security.Claims;

namespace ARSounds.Core.Auth;

public class UserInfo
{
    public IEnumerable<Claim> Claims { get; }

    public UserInfo(IEnumerable<Claim> claims)
    {
        Claims = claims;
    }
}
