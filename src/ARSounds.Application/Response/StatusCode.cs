using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARSounds.Application.Response
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusCode
    {
        Success,
        Failed
    }
}
