namespace ARSounds.Application.Auth;

public class LogoutUserResultDto : ResultDto
{
    public LogoutUserResultDto(bool success) : base(success)
    {
    }

    public LogoutUserResultDto(bool success, string errorMessage) : base(success, errorMessage)
    {
    }
}