using System.Runtime.InteropServices;
using AutoMapper;
using Backend.Models;
using Backend.Repositories;
using DTO.Models;

namespace Backend.Services.DataServices;



public class ProjectService(ProjectRepository repository)
{
   private readonly ProjectRepository _repository = repository;
   private readonly IMapper _mapper;

   public Task<IEnumerable<ProjectsEntity>> GetProjectsWithElectionId(string id)
   {
      return _repository.GetByElectionID(id);
   }

   public async Task CreateProjectAsync(ProjectsEntity project)
   {
      var projectEntity = await _repository.CreateAsync(project);
   }

   public async Task UpdateProjectAsync(ProjectsEntity project)
   {
      await _repository.UpdateAsync(project);
   }

   public async Task DeleteProjectAsync(Guid project_id)
   {
      await _repository.DeleteAsync(project_id);
   }
}