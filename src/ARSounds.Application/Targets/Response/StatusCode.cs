using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARSounds.Application.ImageRecognition.Response
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusCode
    {
        Success,
        Failed
    }
}
