namespace ARSounds.Core.Configuration;

public record OidcConfiguration(string Authority, string ClientId, string Scope, string RedirectUri, string PostLogoutRedirectUri);
