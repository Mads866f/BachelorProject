using System.Runtime.InteropServices;
using AutoMapper;
using Backend.Models;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;

/// <summary>
/// Provides operations for managing projects, including creation, retrieval, update, and deletion.
/// Uses AutoMapper for DTO/entity conversions and delegates data access to the repository layer.
/// </summary>
public class ProjectService(IProjectsRepository repository, IMapper mapper, ILogger<IProjectService> logger) : IProjectService
{
    private readonly IProjectsRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IProjectService> _logger = logger;

    /// <summary>
    /// Retrieves all projects associated with a given election ID.
    /// </summary>
    /// <param name="id">The election ID to filter projects by.</param>
    /// <returns>A collection of project DTOs associated with the election.</returns>
    public async Task<IEnumerable<Project>> GetProjectsWithElectionId(Guid id)
    {
        var result = await _repository.GetByElectionID(id);
        return result.Select(x => _mapper.Map<Project>(x));
    }

    /// <summary>
    /// Retrieves a single project by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the project to retrieve.</param>
    /// <returns>The project DTO if found; otherwise, null.</returns>
    public async Task<Project?> GetProjectByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        return _mapper.Map<Project>(result);
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="createProjectModel">The model containing data for the new project.</param>
    /// <returns>The created project mapped as a DTO.</returns>
    public async Task<Project?> CreateProjectAsync(CreateProjectModel createProjectModel)
    {
        var project = _mapper.Map<ProjectsEntity>(createProjectModel);
        var projectEntity = await _repository.CreateAsync(project);
        return _mapper.Map<Project>(projectEntity);
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="project">The project entity containing updated values.</param>
    public async Task UpdateProjectAsync(ProjectsEntity project)
    {
        await _repository.UpdateAsync(project);
    }

    /// <summary>
    /// Deletes a project by its unique identifier.
    /// </summary>
    /// <param name="projectId">The ID of the project to delete.</param>
    public async Task DeleteProjectAsync(Guid projectId)
    {
        await _repository.DeleteAsync(projectId);
    }
}