using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Front.Services.Interface.Elections;

public interface IProjectsApiService
{
    Task<List<Project>> GetProjectsWithElectionId(Guid id);
    
    Task<int> CreateProject(CreateProjectModel projectToCreate);
    
    Task<int> UpdateProject(Project projectToUpdate);
    
    Task<int> DeleteProject(Project projectToDelete);
    
}