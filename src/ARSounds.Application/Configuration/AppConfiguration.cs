namespace ARSounds.Application.Configuration;

/// <summary>
/// Holds application-wide configuration settings, typically loaded from appsettings.json or environment variables.
/// </summary>
/// <param name="ApplicationName">The name of the application.</param>
/// <param name="ARSoundsApiUrl">The base URL of the ARSounds backend API.</param>
/// <param name="OpenVisionWebSocketUrl">The WebSocket URL used for OpenVision communication.</param>
/// <param name="OpenVisionClientApiKey">The API key used to authenticate with the OpenVision client.</param>
/// <param name="Authority">The authority (issuer) URL for the identity provider (e.g., IdentityServer, Keycloak).</param>
/// <param name="ClientId">The OAuth2/OpenID Connect client ID used for authentication.</param>
/// <param name="RedirectUri">The URI to redirect to after successful authentication.</param>
/// <param name="PostLogoutRedirectUri">The URI to redirect to after logout.</param>
/// <param name="Scope">The scope(s) to request during authentication (space-separated).</param>
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