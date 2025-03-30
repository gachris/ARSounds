using Newtonsoft.Json;

namespace ARSounds.ApiClient.Response;

public class ResponseDoc<TResult>
{
    [JsonProperty("result")]
    public required TResult Result { get; set; }
}