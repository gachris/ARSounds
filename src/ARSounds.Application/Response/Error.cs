using Newtonsoft.Json;

namespace ARSounds.Application.Response;

public class Error
{
    [JsonProperty("result_code")]
    public ResultCode ResultCode { get; set; }

    [JsonProperty("error_message")]
    public required object ErrorMessage { get; set; }
}