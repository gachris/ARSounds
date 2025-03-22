using ARSounds.Web.Api.Core.Enums;
using ARSounds.Web.Api.Core.Utils;
using ARSounds.Web.Api.EntityFramework.Entities;
using ARSounds.Web.Core.Responses;

namespace ARSounds.Web.Api.Core.Converters;

public static class TargetConverter
{
    public static IEnumerable<TargetResponse?> ConvertAllToResponse(this IEnumerable<Target> targets)
    {
        foreach (var target in targets) yield return target?.ConvertToResponse();
    }

    public static TargetResponse ConvertToResponse(this Target target)
    {
        return new TargetResponse(target.Id,
                                  target.Description,
                                  target.Audio.Filename,
                                  target.Audio.AudioType,
                                  target.Audio?.AudioBytes.GetAudioAsBase64((AudioType)target.Audio?.AudioType.ToAudioType(), true),
                                  target.Image?.Buffer.GetImageAsBase64(ImagetType.Jpeg, true),
                                  target.Image?.VisionTargetId,
                                  target.IsActive,
                                  target.IsTrackable,
                                  target.HexColor,
                                  target.Image?.Rate,
                                  target.Created,
                                  target.Updated);
    }
}
