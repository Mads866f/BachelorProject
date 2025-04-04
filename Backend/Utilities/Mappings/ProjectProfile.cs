using AutoMapper;
using Backend.Models;
using DTO.Models;

namespace Backend.Utilities.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectModel, ProjectsEntity>();
        CreateMap<ProjectsEntity, Project>();
        CreateMap<Project, ProjectsEntity>();
    }
}