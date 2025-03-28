using ARSounds.Web.Api.Core.Enums;
using ARSounds.Web.Api.Core.Utils;
using ARSounds.Web.Api.EntityFramework.Entities;
using ARSounds.Web.Core.Responses;
using AutoMapper;

namespace ARSounds.Web.Api.Core.Mappers;

/// <summary>
/// AutoMapper profile for mapping between entity classes and response/DTO classes.
/// </summary>
internal class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// Configures AutoMapper mappings between entity classes and response/DTO classes.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Target, TargetResponse>()
            .ConstructUsing(src => new TargetResponse(
                src.Id,
                src.Description,
                src.Audio.Filename,
                src.Audio.AudioType,
                src.Audio.AudioBytes.GetAudioAsBase64(
                    (AudioType)src.Audio.AudioType.ToAudioType()!,
                    true
                ),
                src.Image == null
                    ? null
                    : src.Image.Buffer.GetImageAsBase64(ImagetType.Jpeg, true),
                src.Image == null
                    ? null
                    : src.Image.VisionTargetId,
                src.IsActive,
                src.IsTrackable,
                src.HexColor,
                src.Image == null
                    ? null
                    : src.Image.Rate,
                src.Created,
                src.Updated
           ));
    }
}