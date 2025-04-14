using AutoMapper;

namespace ARSounds.Application.Mappers.Converters;

public class UserClaimsResolver : ITypeConverter<ApiClient.Data.UserClaimsCollection, Core.ClaimsPrincipal.UserClaims>
{
    public Core.ClaimsPrincipal.UserClaims Convert(ApiClient.Data.UserClaimsCollection source, Core.ClaimsPrincipal.UserClaims destination, ResolutionContext context)
    {
        return new Core.ClaimsPrincipal.UserClaims(
            Id: source.FirstOrDefault(c => c.Type == "sub")?.Value ?? string.Empty,
            Name: source.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty,
            Role: source.FirstOrDefault(c => c.Type == "role")?.Value ?? string.Empty,
            Username: source.FirstOrDefault(c => c.Type == "preferred_username")?.Value ?? string.Empty,
            Email: source.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty,
            EmailVerified: bool.TryParse(source.FirstOrDefault(c => c.Type == "email_verified")?.Value, out var verified) && verified
        );
    }
}