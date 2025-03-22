namespace ARSounds.Application.Targets;

public class GetTargetsResultDto : ResultDto
{
    public GetTargetsResultDto(bool success) : base(success)
    {
    }

    public GetTargetsResultDto(bool success, string errorMessage) : base(success, errorMessage)
    {
    }
}