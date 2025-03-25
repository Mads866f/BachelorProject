using AutoMapper;
using Backend.Models;
using DTO.Models;

namespace Backend.Utilities.Mappings;

public class ScoresProfile : Profile
{
    public ScoresProfile()
    {
        CreateMap<ScoresEntity, Scores>();
        CreateMap<Scores, ScoresEntity>();
    }
}