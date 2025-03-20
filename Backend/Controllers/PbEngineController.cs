using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class PbEngineController
{
    [HttpPost]
    public async Task<IActionResult> TestElection()
    {
        return null;
    }
}