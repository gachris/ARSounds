using ARSounds.Core.Auth;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.Auth;

public interface IAuthService
{
    bool IsUserLoggedIn { get; }
    Token Token { get; }
    UserInfo UserInfo { get; }

    Task LoginAsync(CancellationToken cancellationToken = default);
    Task LogoutAsync(CancellationToken cancellationToken = default);
    Task TryLoginFromCacheAsync(CancellationToken cancellationToken = default);
    Task RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
