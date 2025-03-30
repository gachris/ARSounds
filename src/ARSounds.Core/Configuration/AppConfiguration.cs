namespace ARSounds.Core.Configuration;

public record AppConfiguration(
    string ApplicationName,
    string ARSoundsApiUrl,
    string OpenVisionWebSocketUrl,
    string OpenVisionClientApiKey,
    string Authority,
    string ClientId,
    string RedirectUri,
    string PostLogoutRedirectUri,
    string Scope);
