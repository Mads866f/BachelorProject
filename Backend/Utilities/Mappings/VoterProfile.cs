using AutoMapper;
using Backend.Models;
using DTO.Models;

namespace Backend.Utilities.Mappings;

public class VoterProfile : Profile
{
    public VoterProfile()
    {
        CreateMap<VoteEntity, Voter>();
        CreateMap<Voter, VoteEntity>();
    }
}