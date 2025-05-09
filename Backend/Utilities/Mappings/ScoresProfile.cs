using AutoMapper;
using Backend.Models;
using DTO.Models;

namespace Backend.Utilities.Mappings;

public class ScoresProfile : Profile
{
    public ScoresProfile()
    {
        // Entity -> DTO
        CreateMap<ScoresEntity, Scores>()
            .ForMember(dest => dest.project, 
                opt => 
                    opt.MapFrom(src => src.ProjectsEntity));
        // DTO -> Entity
        CreateMap<Scores, ScoresEntity>()
            .ForMember(dest => dest.ProjectsEntity, 
                opt => 
                    opt.MapFrom(src => src.project));
    }
}