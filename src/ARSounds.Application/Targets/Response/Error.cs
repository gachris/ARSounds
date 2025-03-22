using Newtonsoft.Json;

namespace ARSounds.Application.ImageRecognition.Response
{
    public class Error
    {
        [JsonProperty("result_code")]
        public ResultCode ResultCode { get; set; }

        [JsonProperty("error_message")]
        public object ErrorMessage { get; set; }
    }
}