using Backend.Models;
using Backend.Services.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/Election")]
public class ElectionController(ElectionService service) : ControllerBase
{
    private readonly ElectionService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ElectionEntity>>> GetAll()
    {
        Console.WriteLine("Backend Tries to get All From Database");
        var result = Ok(await _service.GetAllElectionsAsync());
        Console.WriteLine("Controller got:"+result);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ElectionEntity election)
    {
        Console.WriteLine("Backend Controller Create Was Called");
        var createdElection = await _service.CreateElectionAsync(election);
        return Ok(createdElection);
    }
}