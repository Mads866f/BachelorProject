using AutoMapper;
using Backend.Models;
using DTO.Models;

namespace Backend.Utilities.Mappings;

public class VoterProfile : Profile
{
    public VoterProfile()
    {
        // Entity -> DTO
        CreateMap<VoteEntity, Voter>()
            .ForMember(dest => dest.Votes, 
                opt => 
                    opt.MapFrom(src => src.ScoresEntities));
        // DTO -> Entity
        CreateMap<Voter, VoteEntity>()
            .ForMember(dest => dest.ScoresEntities, 
                opt => 
                    opt.MapFrom(src => src.Votes));
    }
}