using ARSounds.Application.Auth;
using ARSounds.Application.Store;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth;
using ARSounds.Core.Auth.Events;
using ARSounds.Core.Configuration;
using IdentityModel.OidcClient;
using System.IO;
using System.Net;
using System.Text;

namespace ARSounds.UI.Wpf.Auth;

public class AuthService : IAuthService
{
    private const string AuthKey = "user";
    private const string CallbackHtml = "<html><head><meta http-equiv='refresh' content='10;url={0}'></head><body>Please return to the app.</body></html>";

    private readonly IDataStore _dataStore;
    private readonly OidcClient _oidcClient;
    private readonly OidcClientOptions _oidcClientOptions;
    private readonly IApplicationEvents _applicationEvents;

    private bool _isUserLoggedIn;
    private Token _token;
    private UserInfo _userInfo;

    public bool IsUserLoggedIn => _isUserLoggedIn;

    public Token Token => _token;

    public UserInfo UserInfo => _userInfo;

    public AuthService(IApplicationEvents applicationEvents,
                       OidcConfiguration oidcConfiguration,
                       AppConfiguration appConfiguration,
                       IDataStore dataStore)
    {
        _oidcClientOptions = CreateOidcClientOptions(oidcConfiguration);
        _oidcClient = new OidcClient(_oidcClientOptions);
        _dataStore = dataStore;
        _applicationEvents = applicationEvents;
    }

    public async Task LoginAsync(CancellationToken cancellationToken = default)
    {
        var loginRequest = new Application.Auth.DataTypes.LoginRequest("http://127.0.0.1:7890/", "https://sts.skoruba.local");

        using var httpListener = new HttpListener();
        httpListener.Prefixes.Add(loginRequest.RedirectUri);
        httpListener.Start();

        var parameters = new IdentityModel.Client.Parameters() { { "response_mode", "form_post" } };
        var authorizeState = await _oidcClient.PrepareLoginAsync(parameters);

        // Open the system browser (Use Process.Start in WPF)
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(authorizeState.StartUrl) { UseShellExecute = true });

        // Wait for the authorization response
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

    private static OidcClientOptions CreateOidcClientOptions(OidcConfiguration oidcConfiguration)
    {
        return new OidcClientOptions
        {
            Authority = oidcConfiguration.Authority,
            Scope = oidcConfiguration.Scope,
            ClientId = oidcConfiguration.ClientId,
            RedirectUri = "http://127.0.0.1:7890/signin-oidc",
            PostLogoutRedirectUri = "http://127.0.0.1:7890/signout-callback-oidc",
            Policy = new Policy
            {
                RequireIdentityTokenSignature = false
            }
        };
    }
}