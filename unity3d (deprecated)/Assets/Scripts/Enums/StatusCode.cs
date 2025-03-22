using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusCode
    {
        Success,
        Failed
    }
}
