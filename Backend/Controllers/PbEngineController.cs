using Backend.Services.Interfaces.PbEngine;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class PbEngineController(IInitialtest apiService)
{
    [HttpPost]
    public async Task TestElection()
    {
        await apiService.TestElection();
    }
}