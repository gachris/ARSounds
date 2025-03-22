using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ARSounds.Application.ImageRecognition.Response
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
        InternalServerError,
        UnknownError
    }
}
