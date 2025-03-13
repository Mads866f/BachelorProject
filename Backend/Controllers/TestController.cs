using Backend.Services.ApiServices.PbEngine;
using Backend.Services.Interfaces.PbEngine;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TestController(IInitialtest testservice) : ControllerBase
{
    private readonly IInitialtest _initialtest = testservice;
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var test = await _initialtest.Test();
        return Ok(test);
    }
}