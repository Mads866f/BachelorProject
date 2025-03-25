using Backend.Models;
using Backend.Services.DataServices;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/Project")]
public class ProjectController(ProjectService service) : ControllerBase
{
    private readonly ProjectService _service = service;

    [HttpGet ("{id}")]
    public async Task<ActionResult<IEnumerable<ProjectsEntity>>> GetByElectionID(string id)
    {
        Console.WriteLine("Getting Projects - Backend(Controller)");
        var result = await _service.GetProjectsWithElectionId(id);
        return result is not null ? Ok(result) : NotFound();
    }
}