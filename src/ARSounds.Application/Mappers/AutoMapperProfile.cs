using ARSounds.ApiClient.Data;
using ARSounds.ApiClient.Dtos;
using ARSounds.Application.Mappers.Converters;
using ARSounds.Core.ClaimsPrincipal;
using ARSounds.Core.Targets;
using AutoMapper;

namespace ARSounds.Application.Mappers;

/// <summary>
/// Defines AutoMapper configuration for mapping between domain and API DTOs.
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
    /// Sets up mappings between <see cref="Target"/> and <see cref="TargetDto"/>.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<Target, TargetDto>()
            .ReverseMap();

        CreateMap<UserClaimsCollection, UserClaims>()
            .ConvertUsing<UserClaimsResolver>();
    }
}
