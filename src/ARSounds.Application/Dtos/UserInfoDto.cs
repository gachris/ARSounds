using System.Security.Claims;

namespace ARSounds.Application.Dtos;

public record UserInfoDto(IEnumerable<Claim> Claims);