using ARSounds.EntityFramework.Entities;
using ARSounds.Server.Core.Dtos;
using ARSounds.Server.Core.GraphQL.Inputs;
using ARSounds.Server.Core.GraphQL.Types;
using ARSounds.Server.Core.Helpers;
using ARSounds.Server.Core.Requests;
using ARSounds.Server.Core.Responses;
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
        CreateMap<AudioAsset, TargetDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Audio, opt => opt.MapFrom(src => src.Audio.GetAsBase64()))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.ImageAsset != null))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageAsset != null ? src.ImageAsset.Image.GetAsBase64() : null))
            .ForMember(dest => dest.OpenVisionId, opt => opt.MapFrom(src => src.ImageAsset != null ? src.ImageAsset.OpenVisionId : null))
            .ForMember(dest => dest.IsTrackable, opt => opt.MapFrom(src => src.ImageAsset != null ? (bool?)src.ImageAsset.IsTrackable : null))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.ImageAsset != null ? (string?)src.ImageAsset.Color : null))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.ImageAsset != null ? (int?)src.ImageAsset.Rate : null))
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
            .ForMember(dest => dest.Updated, opt => opt.MapFrom(src => src.Updated));

        CreateMap<TargetDto, Target>()
            .ReverseMap();

        CreateMap<CreateTargetDto, CreateTargetInput>()
            .ReverseMap();

        CreateMap<UpdateTargetDto, UpdateTargetInput>()
            .ReverseMap();

        CreateMap<ActivateTargetDto, ActivateTargetInput>()
            .ReverseMap();

        CreateMap<TargetDto, TargetResponse>()
            .ReverseMap();

        CreateMap<CreateTargetDto, CreateTargetRequest>()
            .ReverseMap();

        CreateMap<UpdateTargetDto, UpdateTargetRequest>()
            .ReverseMap();

        CreateMap<ActivateTargetDto, ActivateTargetRequest>()
            .ReverseMap();
    }
}