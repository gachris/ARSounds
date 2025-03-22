using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResultCode
    {
        Success,
        InvalidModelProperty,
        InvalidRequest,
        Unauthorized,
        Forbidden,
        RecordNotFound,
        VuforiaResultCodeError,
        VuforiaApiException,
        InternalServerError,
        UnknownError
    }
}
