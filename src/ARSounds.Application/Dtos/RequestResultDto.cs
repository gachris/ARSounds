namespace ARSounds.Application.Dtos;

/// <summary>
/// Represents the result of an operation with an optional error message.
/// </summary>
/// <param name="Error"></param>
/// <param name="Exception"></param>
public record RequestResultDto(string? Error = null, Exception? Exception = null)
{
    #region Properties

    public bool IsSuccess => string.IsNullOrEmpty(Error);

    #endregion

    public RequestResultDto() : this(null, null)
    {
    }
}