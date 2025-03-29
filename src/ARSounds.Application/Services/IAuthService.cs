using ARSounds.Core.Auth;

namespace ARSounds.Application.Services;

public interface IAuthService
{
    bool IsAuthenticated { get; }

    Token? Token { get; }

    UserInfo? UserInfo { get; }

    Task SignInAsync(CancellationToken cancellationToken = default);
    Task SignInSilentAsync(CancellationToken cancellationToken = default);
    Task SignOutAsync(CancellationToken cancellationToken = default);
    Task RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
