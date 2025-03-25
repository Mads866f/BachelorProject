using Backend.Models;
using Backend.Repositories;

namespace Backend.Services.DataServices;



public class ProjectService(ProjectRepository repository)
{
   private readonly ProjectRepository _repository = repository;
   public Task<IEnumerable<ProjectsEntity>> GetProjectsWithElectionId(string id)
   {
      return _repository.GetByElectionID(id);
   }
}