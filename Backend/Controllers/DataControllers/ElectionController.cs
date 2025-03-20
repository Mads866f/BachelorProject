using Backend.Models;
using Backend.Services.DataServices;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/Election")]
public class ElectionController(ElectionService service) : ControllerBase
{
    private readonly ElectionService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Election>>> GetAll()
    {
        Console.WriteLine("Backend Tries to get All From Database");
        var result = await _service.GetAllElectionsAsync();
        List<Election> resultList = result.Select(entity => entity.ToElectionDto()).ToList();
        Console.WriteLine("Controller got:"+result);
        return Ok(resultList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ElectionEntity>> Get(string id)
    {
        var result = await _service.GetElection(id);
        Console.WriteLine(result?.TotalBudget);
        Console.WriteLine(result?.ToElectionDto().TotalBudget);
        return Ok(result?.ToElectionDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Election election)
    {
        Console.WriteLine("Backend Controller Create Was Called");
        var createdElection = await _service.CreateElectionAsync(new ElectionEntity(election));
        return Ok(createdElection);
    }
}