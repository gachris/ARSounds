using Newtonsoft.Json;

namespace Assets
{
    public class ResponseDoc<TResult>
    {
        [JsonProperty("result")]
        public TResult Result { get; set; }
    }
}