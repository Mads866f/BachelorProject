using Backend.Models;
using DTO.Models;

namespace Backend.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetProjectsWithElectionId(Guid id);
    
    Task<Project?> GetProjectByIdAsync(Guid id);


    Task<Project?> CreateProjectAsync(CreateProjectModel createProjectModel);


    Task UpdateProjectAsync(ProjectsEntity project);


    Task DeleteProjectAsync(Guid projectId);

}