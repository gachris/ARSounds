using System.Collections;
using System.Security.Claims;

namespace ARSounds.ApiClient.Data;

/// <summary>
/// Wraps a collection of user claims and provides read-only access.
/// </summary>
public class UserClaimsCollection : IReadOnlyList<Claim>
{
    private readonly List<Claim> _claims;

    public UserClaimsCollection(IEnumerable<Claim> claims)
    {
        _claims = claims?.ToList() ?? [];
    }

    public int Count => _claims.Count;

    public Claim this[int index] => _claims[index];

    public IEnumerator<Claim> GetEnumerator() => _claims.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}