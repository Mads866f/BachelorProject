using Backend.Services.ApiServices.PbEngine;
using Backend.Services.Interfaces.PbEngine;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CalculateElectionController(ICalcElection calculation) : ControllerBase
{
    private readonly ICalcElection _calc = calculation;
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var test = await _calc.CalculateElection(instance,profile);
        return Ok(test);
    }
}