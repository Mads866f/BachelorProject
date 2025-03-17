using Backend.Models;
using Backend.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/[controller]")]
public class ElectionController(ElectionService service) : ControllerBase
{
    private readonly ElectionService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ElectionEntity>>> GetAll() => 
        Ok(await _service.GetAllElectionsAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ElectionEntity election)
    {
        var createdElection = await _service.CreateElectionAsync(election);
        return Ok(createdElection);
    }
}