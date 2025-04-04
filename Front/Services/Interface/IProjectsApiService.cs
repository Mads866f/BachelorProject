using DTO.Models;

namespace Front.Services.Interface.Elections;

public interface IProjectsApiService
{
    Task<List<Project>> GetProjectsWithElectionId(string id);
    
    Task CreateProject(CreateProjectModel projectToCreate);
    
    Task UpdateProject(Project projectToUpdate);
    
    Task DeleteProject(Project projectToDelete);
    
}