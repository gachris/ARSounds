using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace ARSounds.Core.Auth;

public class Token
{
    [JsonProperty("access_token")]
    public virtual string AccessToken { get; set; }

    [JsonProperty("refresh_token")]
    public virtual string RefreshToken { get; set; }

    [JsonProperty("identity_token")]
    public virtual string IdentityToken { get; set; }

    [JsonProperty("expires_in")]
    public virtual int ExpiresIn { get; set; }

    [JsonProperty("access_token_expiration")]
    public virtual DateTimeOffset AccessTokenExpiration { get; set; }

    public static bool IsTokenValid(string accessToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        return !(jwtSecurityTokenHandler.ReadJwtToken(accessToken).ValidTo < DateTime.UtcNow);
    }
}