using System.Runtime.InteropServices;
using AutoMapper;
using Backend.Models;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;



public class ProjectService(IProjectsRepository repository, IMapper mapper) : IProjectService
{
   private readonly IProjectsRepository _repository = repository;
   private readonly IMapper _mapper = mapper;

   public async Task<IEnumerable<Project>> GetProjectsWithElectionId(string id)
   {
      var result = await _repository.GetByElectionID(id);
      Console.WriteLine("RESULT IS null:"+result is null);
      var dto = result.Select(x => { Console.WriteLine("X"+x.Name); return _mapper.Map<Project>(x); });
      return dto;
   }

   public async Task<Project?> CreateProjectAsync(Project project_dto)
   {
      var project = _mapper.Map<ProjectsEntity>(project_dto); 
      var projectEntity = await _repository.CreateAsync(project);
      var dto =  _mapper.Map<Project>(projectEntity); 
      return dto;
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