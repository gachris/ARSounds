using ARSounds.Server.Core.Responses;
using ARSounds.Server.Core.Utils;
using ARSounds.Server.EntityFramework.Entities;
using AutoMapper;

namespace ARSounds.Server.Core.Mappers;

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
                src.Audio.AudioBytes.GetAsBase64(src.Audio.AudioType),
                src.Image == null
                    ? null
                    : src.Image.Buffer.GetAsBase64("image/jpeg"),
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