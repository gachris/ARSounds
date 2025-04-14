using Newtonsoft.Json;

namespace ARSounds.ApiClient.Data;

public class Token
{
    [JsonProperty("access_token")]
    public virtual required string AccessToken { get; set; }

    [JsonProperty("refresh_token")]
    public virtual required string RefreshToken { get; set; }

    [JsonProperty("identity_token")]
    public virtual required string IdentityToken { get; set; }

    [JsonProperty("expires_in")]
    public virtual int ExpiresIn { get; set; }

    [JsonProperty("access_token_expiration")]
    public virtual DateTimeOffset AccessTokenExpiration { get; set; }
}