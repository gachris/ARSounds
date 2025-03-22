using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class UnityAuthClient
    {
        private OidcClient _client;
        private IDataStore _dataStore;
        private IConfiguration _configuration;
        private readonly BrowserBase _browser;

        public UnityAuthClient()
        {
            // We must disable the IdentityModel log serializer to avoid Json serialize exceptions on IOS.
#if UNITY_IOS
            IdentityModel.OidcClient.Infrastructure.LogSerializer.Enabled = false;
#endif
            _browser = GetBrowser();
            CertificateHandler.Initialize();
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                _dataStore = new FileDataStore("tokens");
                _configuration = AppBuilder.GetConfiguration();

                var oidcClientOptionsConfiguration = _configuration.GetSection(nameof(OidcClientOptionsConfiguration)).Get<OidcClientOptionsConfiguration>();
                var options = new OidcClientOptions()
                {
                    Authority = oidcClientOptionsConfiguration.Authority,
                    ClientId = oidcClientOptionsConfiguration.ClientId,
                    Scope = string.Join(" ", oidcClientOptionsConfiguration.Scopes),
                    RedirectUri = oidcClientOptionsConfiguration.RedirectUri,
                    PostLogoutRedirectUri = oidcClientOptionsConfiguration.PostLogoutRedirectUri,
                    Browser = _browser
                };
                options.LoggerFactory.AddProvider(new UnityAuthLoggerProvider());
                _client = new OidcClient(options);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", true, null);
                Debug.Log("UnityAuthClient::Exception during Initialize: " + ex.Message);
            }
        }

        public async Task<Token> LoginAsync()
        {
            LoginResult loginResult = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
#if UNITY_STANDALONE
                var oidcClientOptionsConfiguration = _configuration.GetSection(nameof(OidcClientOptionsConfiguration)).Get<OidcClientOptionsConfiguration>();
                loginResult = await _client.LoginAsync(new Assets.LoginRequest(oidcClientOptionsConfiguration.ListenerUri, oidcClientOptionsConfiguration.Authority));
#else
                loginResult = await _client.LoginAsync(new IdentityModel.OidcClient.LoginRequest());
#endif
            }
            catch (Exception e)
            {
                Debug.Log("UnityAuthClient::Exception during login: " + e.Message);
                return null;
            }
            finally
            {
                if (_browser != null)
                {
                    Debug.Log("UnityAuthClient::Dismissing sign-in browser.");
                    _browser.Dismiss();
                }
            }

            if (loginResult.IsError)
            {
                Debug.Log("UnityAuthClient::Error authenticating: " + loginResult.Error);
            }
            else
            {
                Debug.Log("UnityAuthClient::Signed in.");
                Debug.Log("UnityAuthClient::AccessToken: " + loginResult.AccessToken);
                Debug.Log("UnityAuthClient::RefreshToken: " + loginResult.RefreshToken);
                Debug.Log("UnityAuthClient::IdentityToken: " + loginResult.IdentityToken);
                foreach (var claim in loginResult.User.Claims)
                {
                    Debug.Log($"UnityAuthClient::Claim: {claim.Type}: {claim.Value}");
                    Console.WriteLine();
                }

                var token = new Token()
                {
                    AccessToken = loginResult.AccessToken,
                    IdentityToken = loginResult.IdentityToken,
                    RefreshToken = loginResult.RefreshToken,
                    AccessTokenExpiration = loginResult.AccessTokenExpiration,
                    ExpiresIn = loginResult.TokenResponse.ExpiresIn,
                };

                await _dataStore.StoreAsync("user", token);

                return token;
            }

            await _dataStore.DeleteAsync<Token>("user");

            return null;
        }

        public async Task<bool> LogoutAsync(string identityToken)
        {
            LogoutResult logoutResult = null;
            try
            {
#if UNITY_STANDALONE
                logoutResult = await _client.LogoutAsync(new Assets.LogoutRequest()
                {
                    IdTokenHint = identityToken
                });
#else
                logoutResult = await _client.LogoutAsync(new IdentityModel.OidcClient.LogoutRequest()
                {
                    BrowserDisplayMode = IdentityModel.OidcClient.Browser.DisplayMode.Hidden,
                    IdTokenHint = identityToken
                });
#endif
                await _dataStore.DeleteAsync<Token>("user");
                Debug.Log($"UnityAuthClient::{logoutResult.Response}");
                Debug.Log("UnityAuthClient::Signed out successfully.");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("UnityAuthClient::Failed to sign out: " + e.Message);
            }
            finally
            {
                Debug.Log("UnityAuthClient::Dismissing sign-out browser.");
                if (_browser != null)
                {
                    _browser.Dismiss();
                }
            }

            return false;
        }

        public async Task<Token> RefreshToken(string refreshToken)
        {
            try
            {
                var refreshTokenResponse = await _client.RefreshTokenAsync(refreshToken);

                return new Token()
                {
                    AccessToken = refreshTokenResponse.AccessToken,
                    IdentityToken = refreshTokenResponse.IdentityToken,
                    RefreshToken = refreshTokenResponse.RefreshToken,
                    AccessTokenExpiration = refreshTokenResponse.AccessTokenExpiration,
                    ExpiresIn = refreshTokenResponse.ExpiresIn,
                };
            }
            catch (Exception e)
            {
                Debug.Log("UnityAuthClient::Failed to refresh token: " + e.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Claim>> GetUserInfoAsync(string accessToken)
        {
            UserInfoResult userInfoResult = null;
            try
            {
                userInfoResult = await _client.GetUserInfoAsync(accessToken);
            }
            catch (Exception e)
            {
                Debug.Log("UnityAuthClient::Failed to get user info: " + e.Message);
            }

            if (userInfoResult.IsError)
            {
                Debug.Log("UnityAuthClient::Error get user info: " + userInfoResult.Error);
            }
            else
            {
                return userInfoResult.Claims;
            }

            return null;
        }

        public async Task<Token> GetToken()
        {
            try
            {
                var token = await _dataStore?.GetAsync<Token>("user");
                if (token != null)
                {
                    var isTokenValid = token.IsTokenValid(token.AccessToken);
                    return !isTokenValid ? await RefreshToken(token.RefreshToken) : token;
                }
            }
            catch (Exception e)
            {
                Debug.Log("UnityAuthClient::Failed to get token: " + e.Message);
            }

            return null;
        }

        private BrowserBase GetBrowser()
        {
#if UNITY_STANDALONE
            return null;
#elif UNITY_ANDROID
            return new AndroidChromeCustomTabBrowser();
#elif UNITY_IOS
            return new SFSafariViewBrowser();
#endif
        }

        public void OnAuthReply(string value)
        {
            if (_browser != null)
            {
                _browser.OnAuthReply(value);
            }
        }
    }
}
