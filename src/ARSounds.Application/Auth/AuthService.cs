using ARSounds.Application.Store;
using ARSounds.Core;
using ARSounds.Core.Auth;
using ARSounds.Core.Auth.Events;
using ARSounds.Core.Configuration;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System;
using System.Threading;
using System.Threading.Tasks;
#if WINDOWS
using System.IO;
using System.Net;
using System.Text;
#endif

namespace ARSounds.Application.Auth;

public class AuthService : IAuthService
{
    private const string AuthKey = "user";

    private readonly IDataStore _dataStore;
    private readonly OidcClient _oidcClient;
    private readonly OidcClientOptions _oidcClientOptions;
    private readonly IApplicationEvents _applicationEvents;
#if WINDOWS
    private const string CallbackHtml = "<html><head><meta http-equiv='refresh' content='10;url={0}'></head><body>Please return to the app.</body></html>";
#endif

    private bool _isUserLoggedIn;
    private Token _token;
    private UserInfo _userInfo;

    public bool IsUserLoggedIn => _isUserLoggedIn;

    public Token Token => _token;

    public UserInfo UserInfo => _userInfo;

    public AuthService(IdentityModel.OidcClient.Browser.IBrowser browser,
                       IApplicationEvents applicationEvents,
                       OidcConfiguration oidcConfiguration,
                       AppConfiguration appConfiguration)
    {
        _oidcClientOptions = CreateOidcClientOptions(browser, oidcConfiguration);
        _oidcClient = new OidcClient(_oidcClientOptions);
        _dataStore = new FileDataStore(appConfiguration.ApplicationName);
        _applicationEvents = applicationEvents;
    }

    public async Task LoginAsync(CancellationToken cancellationToken = default)
    {
#if WINDOWS
        var loginRequest = new DataTypes.LoginRequest("http://127.0.0.1:7890/", "https://sts.skoruba.local");

        // create an HttpListener to listen for requests on that redirect URI.
        using var httpListener = new HttpListener();
        httpListener.Prefixes.Add(loginRequest.RedirectUri);

        httpListener.Start();
        var parameters = new IdentityModel.Client.Parameters() { { "response_mode", "form_post" } };
        var authorizeState = await _oidcClient.PrepareLoginAsync(parameters);

        // open system browser to start authentication
        await Microsoft.Maui.ApplicationModel.Browser.OpenAsync(authorizeState.StartUrl, Microsoft.Maui.ApplicationModel.BrowserLaunchMode.SystemPreferred);

        // wait for the authorization response.
        var httpListenerContext = await httpListener.GetContextAsync();

        var body = httpListenerContext.Request.InputStream;
        var streamReader = new StreamReader(body, httpListenerContext.Request.ContentEncoding);
        var formData = streamReader.ReadToEnd();

        // sends an HTTP response to the browser.
        using (var httpListenerResponse = httpListenerContext.Response)
        {
            var buffer = Encoding.UTF8.GetBytes(string.Format(CallbackHtml, loginRequest.CallbackUri));

            httpListenerResponse.ContentLength64 = buffer.Length;

            using var responseOutput = httpListenerResponse.OutputStream;
            await responseOutput.WriteAsync(buffer);
        }

        var loginResult = await _oidcClient.ProcessResponseAsync(formData, authorizeState);

        httpListener.Stop();
#else
        var loginResult = await _oidcClient.LoginAsync(new LoginRequest(), cancellationToken);
#endif
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
        _applicationEvents.Raise(new UserLoggedInEvent());
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
#if WINDOWS
        var logoutUrl = await _oidcClient.PrepareLogoutAsync(new LogoutRequest()
        {
            IdTokenHint = _token.IdentityToken
        });

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(logoutUrl);
        httpWebRequest.Method = "GET";

        var httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();

        var logoutResult = httpWebResponse.StatusCode == HttpStatusCode.OK
            ? new LogoutResult() { Response = "Success" }
            : new LogoutResult(httpWebResponse.StatusCode.ToString());
#else
        var logoutResult = await _oidcClient.LogoutAsync(new LogoutRequest()
        {
            IdTokenHint = _token.IdentityToken,
            BrowserDisplayMode = DisplayMode.Hidden
        }, cancellationToken);
#endif
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

            _applicationEvents.Raise(new UserLoggedOutEvent());
        }
    }

    public async Task TryLoginFromCacheAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await _dataStore.GetAsync<Token>(AuthKey);

            if (token != null)
            {
                var refreshCache = false;

                if (!Token.IsTokenValid(token.AccessToken))
                {
                    var refreshTokenResult = await _oidcClient.RefreshTokenAsync(token.RefreshToken, cancellationToken: cancellationToken);

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

                var userInfoResult = await _oidcClient.GetUserInfoAsync(token.AccessToken, cancellationToken);

                if (userInfoResult.IsError) throw new Exception(userInfoResult.Error);

                if (refreshCache)
                {
                    await _dataStore.StoreAsync(AuthKey, token);
                }

                _token = token;
                _userInfo = new UserInfo(userInfoResult.Claims);
                _isUserLoggedIn = true;

                _applicationEvents.Raise(new UserLoggedInEvent());
            }
        }
        catch
        {
            await _dataStore.DeleteAsync<Token>(AuthKey);
        }
    }

    public async Task RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (!IsUserLoggedIn) throw new Exception("User authentication failed: User is not logged in.");

        var refreshTokenResult = await _oidcClient.RefreshTokenAsync(refreshToken, cancellationToken: cancellationToken);

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

    private static OidcClientOptions CreateOidcClientOptions(IdentityModel.OidcClient.Browser.IBrowser browser, OidcConfiguration oidcConfiguration)
    {
        return new OidcClientOptions
        {
            Authority = oidcConfiguration.Authority,
            Scope = oidcConfiguration.Scope,
            ClientId = oidcConfiguration.ClientId,
#if WINDOWS
            RedirectUri = "http://127.0.0.1:7890/signin-oidc",
            PostLogoutRedirectUri = "http://127.0.0.1:7890/signout-callback-oidc",
#else
            RedirectUri = oidcConfiguration.RedirectUri,
            PostLogoutRedirectUri = oidcConfiguration.PostLogoutRedirectUri,
            Browser = browser,
#endif
            Policy = new Policy
            {
                RequireIdentityTokenSignature = false
            }
        };
    }
}
