using System.ComponentModel;
using ARSounds.Web.Api.Core.Attributes;

namespace ARSounds.Web.Api.Core.Enums;

public enum ImagetType
{
    [Description("image/jpeg")]
    [Base64Prefix("data:image/jpeg;base64,")]
    Jpeg = 0,
    [Description("image/png")]
    [Base64Prefix("data:image/png;base64,")]
    Png = 1
}
