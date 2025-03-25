using ARSounds.Application.Common.Dtos;
using CommonServiceLocator;

namespace ARSounds.Application;

public abstract class ResultDto : BaseResultDto
{
    public bool Success { get; }

    public string ErrorMessage { get; }

    public ResultDto(bool success) => Success = success;

    public ResultDto(bool success, string errorMessage) => (Success, ErrorMessage) = (success, errorMessage);
}
