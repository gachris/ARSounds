namespace ARSounds.Application.Auth;

public class SignInUserResultDto : ResultDto
{
    public SignInUserResultDto(bool success) : base(success)
    {
    }

    public SignInUserResultDto(bool success, string errorMessage) : base(success, errorMessage)
    {
    }
}
