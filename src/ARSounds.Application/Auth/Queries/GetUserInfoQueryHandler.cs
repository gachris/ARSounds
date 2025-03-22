using ARSounds.Core.Auth;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ARSounds.Application.Auth.Queries;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoDto>
{
    #region Fields/Consts

    private readonly IAuthService _authService;

    #endregion

    public GetUserInfoQueryHandler(IAuthService authService) => _authService = authService;

    public Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        UserInfoDto userInfoDto = _authService.UserInfo.ToDto(); 
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