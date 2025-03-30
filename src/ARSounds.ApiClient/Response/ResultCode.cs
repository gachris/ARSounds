using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARSounds.ApiClient.Response;

[JsonConverter(typeof(StringEnumConverter))]
public enum ResultCode
{
    Success,
    InvalidModelProperty,
    InvalidRequest,
    Unauthorized,
    Forbidden,
    RecordNotFound,
    InternalServerError,
    UnknownError
}
