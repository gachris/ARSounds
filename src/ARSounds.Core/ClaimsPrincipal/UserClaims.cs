namespace ARSounds.Core.ClaimsPrincipal;

public record UserClaims(
    string Id,
    string Name,
    string Role,
    string Username,
    string Email,
    bool EmailVerified);