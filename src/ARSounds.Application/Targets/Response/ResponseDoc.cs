using Newtonsoft.Json;

namespace ARSounds.Application.ImageRecognition.Response
{
    public class ResponseDoc<TResult>
    {
        [JsonProperty("result")]
        public TResult Result { get; set; }
    }
}