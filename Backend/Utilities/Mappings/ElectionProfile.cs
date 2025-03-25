using AutoMapper;
using Backend.Models;
using DTO.Models;

namespace Backend.Utilities.Mappings;

public class ElectionProfile : Profile
{
    public ElectionProfile()
    {
        CreateMap<ElectionEntity, Election>();
        CreateMap<Election, ElectionEntity>();
    }
}