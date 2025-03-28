using System.Text;
using Backend.Models;
using DTO.Models;

namespace Backend.Repositories.Interfaces;

public interface IProjectsRepository
{ 
   Task<IEnumerable<ProjectsEntity>> GetByElectionID(string electionID);
   
   Task<Project> CreateAsync(ProjectsEntity project);

   Task<IEnumerable<ProjectsEntity>> UpdateAsync(ProjectsEntity project);

   Task<bool> DeleteAsync(Guid project_id);

}