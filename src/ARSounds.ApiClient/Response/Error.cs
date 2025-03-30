using Newtonsoft.Json;

namespace ARSounds.ApiClient.Response;

public class Error
{
    [JsonProperty("result_code")]
    public ResultCode ResultCode { get; set; }

    [JsonProperty("error_message")]
    public required object ErrorMessage { get; set; }
}