using Newtonsoft.Json;

namespace ARSounds.Application.Response;

public class ResponseDoc<TResult>
{
    [JsonProperty("result")]
    public required TResult Result { get; set; }
}