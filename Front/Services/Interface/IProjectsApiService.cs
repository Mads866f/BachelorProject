using DTO.Models;

namespace Front.Services.Interface.Elections;

public interface IProjectsApiService
{
    Task<List<Project>> GetProjectsWithProjectId(string id);
    Task CreateProject(Project projectToCreate);
    
    Task UpdateProject(Project projectToUpdate);
    
    Task DeleteProject(Project projectToDelete);
}