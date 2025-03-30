using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARSounds.ApiClient.Response;

[JsonConverter(typeof(StringEnumConverter))]
public enum StatusCode
{
    Success,
    Failed
}
