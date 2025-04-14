using ARSounds.ApiClient.Data;

namespace ARSounds.ApiClient.Contracts;

public interface IAuthService
{
    bool IsAuthenticated { get; }

    Token? Token { get; }

    UserClaimsCollection? UserClaims { get; }

    Task SignInAsync(CancellationToken cancellationToken = default);

    Task SignInSilentAsync(CancellationToken cancellationToken = default);

    Task SignOutAsync(CancellationToken cancellationToken = default);

    Task RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
