using Backend.Models;
using DTO.Models;

namespace Backend.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetProjectsWithElectionId(string id);


    Task<Project?> CreateProjectAsync(Project project_dto);


    Task UpdateProjectAsync(ProjectsEntity project);


    Task DeleteProjectAsync(Guid project_id);

}