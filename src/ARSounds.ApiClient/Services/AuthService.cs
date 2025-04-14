using System.IdentityModel.Tokens.Jwt;
using ARSounds.ApiClient.Contracts;
using ARSounds.ApiClient.Data;
using ARSounds.ApiClient.DataStore;
using IdentityModel.OidcClient;
using Microsoft.Extensions.Options;

namespace ARSounds.ApiClient.Services;

public class AuthService : IAuthService
{
    #region Fields/Consts

    private const string _authKey = "user";

    private readonly IDataStore _dataStore;
    private readonly OidcClient _client;

    private Token? _token;
    private UserClaimsCollection? _userClaims;

    #endregion

    #region Properties

    public bool IsAuthenticated => Token != null;

    public Token? Token => _token;

    public UserClaimsCollection? UserClaims => _userClaims;

    #endregion

    public AuthService(IDataStore dataStore, IOptions<OidcClientOptions> options)
    {
        _client = new OidcClient(options.Value);
        _dataStore = dataStore;
    }

    public async Task SignInAsync(CancellationToken cancellationToken = default)
    {
        var loginResult = await _client.LoginAsync(new LoginRequest(), cancellationToken);

        if (loginResult.IsError) throw new Exception(loginResult.Error);

        var token = new Token()
        {
            AccessToken = loginResult.AccessToken,
            IdentityToken = loginResult.IdentityToken,
            RefreshToken = loginResult.RefreshToken,
            AccessTokenExpiration = loginResult.AccessTokenExpiration,
            ExpiresIn = loginResult.TokenResponse.ExpiresIn,
        };

        await _dataStore.StoreAsync(_authKey, token);

        _token = token;
        _userClaims = new UserClaimsCollection(loginResult.User.Claims);
    }

    public async Task SignInSilentAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await _dataStore.GetAsync<Token>(_authKey);

            if (token != null)
            {
                var refreshCache = false;

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var isTokenValid = jwtSecurityTokenHandler.ReadJwtToken(token.AccessToken).ValidTo >= DateTime.UtcNow;

                if (!isTokenValid)
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
                    await _dataStore.StoreAsync(_authKey, token);
                }

                _token = token;
                _userClaims = new UserClaimsCollection(userInfoResult.Claims);
            }
        }
        catch
        {
            await _dataStore.DeleteAsync<Token>(_authKey);
        }
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
            _userClaims = null;

            await _dataStore.DeleteAsync<Token>(_authKey);
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
}