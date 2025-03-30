using Newtonsoft.Json;

namespace ARSounds.ApiClient.Response;

public class ErrorResponseMessage : ResponseMessage, IErrorResponseMessage
{
    [JsonProperty("errors")]
    public List<Error> Errors { get; }

    public ErrorResponseMessage()
    {
        Errors = new List<Error>();
    }
}
