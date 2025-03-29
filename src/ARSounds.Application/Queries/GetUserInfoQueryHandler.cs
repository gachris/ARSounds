using ARSounds.Application.Dtos;
using ARSounds.Application.Services;
using ARSounds.Core.Auth;
using MediatR;

namespace ARSounds.Application.Queries;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;

    #endregion

    public GetUserInfoQueryHandler(IAuthService authService) => _authService = authService;

    public Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userInfoDto = _authService.UserInfo?.ToDto() ?? new UserInfoDto([]);
        return Task.FromResult(userInfoDto);
    }
}

static class UserInfoMap
{
    public static UserInfoDto ToDto(this UserInfo userInfo)
    {
        var userInfoDto = new UserInfoDto(userInfo.Claims);
        return userInfoDto;
    }
}