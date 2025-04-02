using System.ComponentModel;
using ARSounds.Server.Core.Attributes;

namespace ARSounds.Server.Core.Enums;

public enum AudioType
{
    [Description("audio/mp3")]
    [Base64Prefix("data:audio/mp3;base64,")]
    Mp3 = 0,
    [Description("audio/mpeg")]
    [Base64Prefix("data:audio/mpeg;base64,")]
    Mpeg = 1,
    [Description("audio/mpeg4")]
    [Base64Prefix("data:video/mpeg4;base64,")]
    Mpeg4 = 2,
    [Description("video/ogg")]
    [Base64Prefix("data:video/ogg;base64,")]
    Ogg = 3,
    [Description("video/h264")]
    [Base64Prefix("data:video/h264;base64,")]
    H264 = 4,
    [Description("audio/wav")]
    [Base64Prefix("data:audio/wav;base64,")]
    Wav = 5,
}
