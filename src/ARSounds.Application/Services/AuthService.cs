using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth;
using ARSounds.Core.Auth.Events;
using ARSounds.Core.Configuration;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;

namespace ARSounds.Application.Services;

public class AuthService : IAuthService
{
    private const string AuthKey = "user";

    private readonly IDataStore _dataStore;
    private readonly OidcClient _client;
    private readonly OidcClientOptions _clientOptions;
    private readonly IApplicationEvents _applicationEvents;

    private bool _isUserLoggedIn;
    private Token? _token;
    private UserInfo? _userInfo;

    public bool IsAuthenticated => _isUserLoggedIn;

    public Token? Token => _token;

    public UserInfo? UserInfo => _userInfo;

    public AuthService(
        IApplicationEvents applicationEvents,
        AppConfiguration appConfiguration,
        IBrowser browser,
        IDataStore dataStore)
    {
        _clientOptions = CreateClientOptions(browser, appConfiguration);
        _client = new OidcClient(_clientOptions);
        _dataStore = dataStore;
        _applicationEvents = applicationEvents;
    }

    public async Task SignInAsync(CancellationToken cancellationToken = default)
    {
        var loginResult = await _client.LoginAsync(new IdentityModel.OidcClient.LoginRequest(), cancellationToken);

        if (loginResult.IsError) throw new Exception(loginResult.Error);

        var token = new Token()
        {
            AccessToken = loginResult.AccessToken,
            IdentityToken = loginResult.IdentityToken,
            RefreshToken = loginResult.RefreshToken,
            AccessTokenExpiration = loginResult.AccessTokenExpiration,
            ExpiresIn = loginResult.TokenResponse.ExpiresIn,
        };

        await _dataStore.StoreAsync(AuthKey, token);

        _token = token;
        _userInfo = new UserInfo(loginResult.User.Claims);

        _isUserLoggedIn = true;
        _applicationEvents.Raise(new SignedInEvent());
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        var logoutResult = await _client.LogoutAsync(new LogoutRequest()
        {
            IdTokenHint = _token?.IdentityToken
        }, cancellationToken);

        try
        {
            if (logoutResult.IsError) throw new Exception(logoutResult.Error);
        }
        finally
        {
            _token = null;
            _userInfo = null;
            _isUserLoggedIn = false;

            await _dataStore.DeleteAsync<Token>(AuthKey);

            _applicationEvents.Raise(new SignedOutEvent());
        }
    }

    public async Task SignInSilentAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await _dataStore.GetAsync<Token>(AuthKey);

            if (token != null)
            {
                var refreshCache = false;

                if (!Token.IsTokenValid(token.AccessToken))
                {
                    var refreshTokenResult = await _client.RefreshTokenAsync(token.RefreshToken, cancellationToken: cancellationToken);

                    if (refreshTokenResult.IsError) throw new Exception(refreshTokenResult.Error);

                    token = new Token()
                    {
                        AccessToken = refreshTokenResult.AccessToken,
                        IdentityToken = refreshTokenResult.IdentityToken,
                        RefreshToken = refreshTokenResult.RefreshToken,
                        AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration,
                        ExpiresIn = refreshTokenResult.ExpiresIn,
                    };

                    refreshCache = true;
                }

                var userInfoResult = await _client.GetUserInfoAsync(token.AccessToken, cancellationToken);

                if (userInfoResult.IsError) throw new Exception(userInfoResult.Error);

                if (refreshCache)
                {
                    await _dataStore.StoreAsync(AuthKey, token);
                }

                _token = token;
                _userInfo = new UserInfo(userInfoResult.Claims);
                _isUserLoggedIn = true;

                _applicationEvents.Raise(new SignedInEvent());
            }
        }
        catch
        {
            await _dataStore.DeleteAsync<Token>(AuthKey);
        }
    }

    public async Task RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated) throw new Exception("User authentication failed: User is not logged in.");

        var refreshTokenResult = await _client.RefreshTokenAsync(refreshToken, cancellationToken: cancellationToken);

        if (refreshTokenResult.IsError) throw new Exception(refreshTokenResult.Error);

        _token = new Token()
        {
            AccessToken = refreshTokenResult.AccessToken,
            IdentityToken = refreshTokenResult.IdentityToken,
            RefreshToken = refreshTokenResult.RefreshToken,
            AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration,
            ExpiresIn = refreshTokenResult.ExpiresIn,
        };
    }

    private static OidcClientOptions CreateClientOptions(IBrowser browser, AppConfiguration appConfiguration)
    {
        return new OidcClientOptions
        {
            Authority = appConfiguration.Authority,
            Scope = appConfiguration.Scope,
            ClientId = appConfiguration.ClientId,
            RedirectUri = appConfiguration.RedirectUri,
            PostLogoutRedirectUri = appConfiguration.PostLogoutRedirectUri,
            Browser = browser,
            Policy = new Policy
            {
                RequireIdentityTokenSignature = false
            }
        };
    }
}