using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/voters")]
public class VotersController(IVotersService voterService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await voterService.GetAllVotersAsync();
        return result.Any() ? Ok(result) : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Voter>> GetAsync(string id)
    {
        var result = await voterService.GetVoterAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVoter voter)
    {
        Voter createdVoter = await voterService.CreateVoterAsync(voter);
        return Ok(createdVoter);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Voter voter)
    {
        var result = await voterService.UpdateVoterAsync(voter);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteByIdAsync(string id)
    {
        var result = await voterService.DeleteByIdAsync(id);
        return result ? Ok() : NotFound();
    }

}
