using System.Collections.Generic;
using System.Security.Claims;

namespace ARSounds.Application.Auth;

public class UserInfoDto
{
    public UserInfoDto(IEnumerable<Claim> claims)
    {
        Claims = claims;
    }

    public IEnumerable<Claim> Claims { get; }
}