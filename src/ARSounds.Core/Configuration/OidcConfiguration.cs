namespace ARSounds.Core.Configuration;

public class OidcConfiguration
{
    public string Authority { get; set; }

    public string ClientId { get; set; }

    public string Scope { get; set; }

    public string RedirectUri { get; set; }

    public string PostLogoutRedirectUri { get; set; }
}