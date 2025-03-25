using Backend.Models;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/[controller]")]
public class ElectionController(IElectionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Election>>> GetAll()
    {
        var result = await service.GetAllElectionsAsync();
        return result.Count() > 1 ? Ok(result) : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Election>> Get(string id)
    {
        var result = await service.GetElectionAsync(id);
        return result is not null? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateElectionModel election)
    {
        Election createdElection = await service.CreateElectionAsync(election);
        return Ok(createdElection);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Election electionModel)
    {
        var result = await service.UpdateElectionAsync(electionModel);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsync(string id)
    {
        var result = await service.DeleteByIdAsync(id);
        return result ? Ok() : NotFound();
    }
}