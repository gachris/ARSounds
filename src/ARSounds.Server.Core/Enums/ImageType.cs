using System.ComponentModel;
using ARSounds.Server.Core.Attributes;

namespace ARSounds.Server.Core.Enums;

public enum ImageType
{
    [Description("image/jpeg")]
    [Base64Prefix("data:image/jpeg;base64,")]
    Jpeg = 0,
    [Description("image/png")]
    [Base64Prefix("data:image/png;base64,")]
    Png = 1
}
