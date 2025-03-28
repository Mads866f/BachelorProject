using Backend.Models;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(IProjectService service) : ControllerBase
{
    private readonly IProjectService _service = service;

    [HttpGet ("{id}")]
    public async Task<ActionResult<IEnumerable<ProjectsEntity>>> GetByElectionID(string id)
    {
        Console.WriteLine("Getting Projects - Backend(Controller)");
        var result = await _service.GetProjectsWithElectionId(id);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateProject([FromBody] Project project)
    {
        Console.WriteLine("Creating Project - Backend(Controller)");
        await _service.CreateProjectAsync(project);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProject([FromBody] ProjectsEntity project)
    {
        Console.WriteLine("Updating Project - Backend(Controller)");
        await _service.UpdateProjectAsync(project);
        return Ok();
    }

    [HttpDelete("{project_id}")]
    public async Task<ActionResult> DeleteProject(Guid project_id)
    {
        await _service.DeleteProjectAsync(project_id);
        return Ok();

    }
    
}