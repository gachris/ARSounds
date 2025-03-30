namespace ARSounds.ApiClient.Response;

public interface IErrorResponseMessage : IResponseMessage
{
    List<Error> Errors { get; }
}