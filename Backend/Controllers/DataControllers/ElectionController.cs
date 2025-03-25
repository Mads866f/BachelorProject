using Backend.Models;
using Backend.Services.DataServices;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/[controller]")]
public class ElectionController(ElectionService service) : ControllerBase
{
    private readonly ElectionService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Election>>> GetAll()
    {
        var result = await _service.GetAllElectionsAsync();
        return result.Count() > 1 ? Ok(result) : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Election>> Get(string id)
    {
        var result = await _service.GetElectionAsync(id);
        return result is not null? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateElectionModel election)
    {
        Election createdElection = await _service.CreateElectionAsync(election);
        return Ok(createdElection);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Election electionModel)
    {
        var result = await _service.UpdateElectionAsync(electionModel);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsync(string id)
    {
        var result = await _service.DeleteByIdAsync(id);
        return result ? Ok() : NotFound();
    }
}