using Backend.Models;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(IProjectService service, ILogger<ProjectController> logger) : ControllerBase
{
    private readonly IProjectService _service = service;
    private readonly ILogger<ProjectController> _logger = logger;

    /// <summary>
    /// Gets associated projects by election ID.
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> of the parent election.</param>
    /// <returns>A list of <see cref="Project"/>s associated with the given election ID.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IEnumerable<ProjectsEntity>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Project>>> GetByElectionId(Guid id)
    {
        _logger.LogInformation("Getting projects for election {ElectionId}", id);

        try
        {
            var result = await _service.GetProjectsWithElectionId(id);
            bool notEmptyList = result.Any();
            if (notEmptyList) return Ok(result);
            _logger.LogWarning("No projects found for election {ElectionId}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting projects for election {ElectionId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="project">The <see cref="Project"/> details to create.</param>
    /// <returns>A response indicating the success or failure of the <see cref="Project"/> creation.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectModel project)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when trying to create project.");
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating project with name {ProjectName}", project.Name);
        try
        {
            await _service.CreateProjectAsync(project);
            _logger.LogInformation("Project created successfully with name {ProjectName}", project.Name);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating project {ProjectName}", project.Name);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    /// <param name="project">The <see cref="Project"/> details to update.</param>
    /// <returns>A response indicating the success or failure of the <see cref="Project"/> update.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateProject([FromBody] ProjectsEntity project)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when trying to update project with ID {ProjectId}", project.Id);
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating project with ID {ProjectId}", project.Id);
        try
        {
            await _service.UpdateProjectAsync(project);
            _logger.LogInformation("Project updated successfully with ID {ProjectId}", project.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating project with ID {ProjectId}", project.Id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deletes a project.
    /// </summary>
    /// <param name="projectId">The <see cref="Guid"/> of the project to delete.</param>
    /// <returns>A response indicating the success or failure of the project deletion.</returns>
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteProject(Guid projectId)
    {
        _logger.LogInformation("Deleting project with ID {ProjectId}", projectId);
        try
        {
            var project = await _service.GetProjectByIdAsync(projectId); 
            if (project == null)
            {
                _logger.LogWarning("Project with ID {ProjectId} not found", projectId);
                return NotFound();
            }

            await _service.DeleteProjectAsync(projectId);
            _logger.LogInformation("Project with ID {ProjectId} deleted successfully", projectId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting project with ID {ProjectId}", projectId);
            return StatusCode(500, "Internal server error");
        }
    }
}
