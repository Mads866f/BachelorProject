using Backend.Models;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.DataServices;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;


[ApiController]
[Route("api/[controller]")]
public class ElectionResultController(ElectionResultService service,ILogger<ElectionResultController> logger) : ControllerBase
{
    /// <summary>
    ///  Redirects the call to respond with a list of results for the election
    /// </summary>
    /// <param name="electionId">
    /// The id Of the election
    /// </param>
    /// <returns>
    /// A list of results for the Elecitons
    /// </returns>
    [HttpGet("{electionId}")]
    public async Task<ActionResult<List<ElectionResult>>> GetResultsByElectionId(Guid electionId)
    {
        logger.LogInformation("Get results for electionId {electionId}", electionId);
        try
        {
            var result = await service.GetElectionsResultsByElectionId(electionId);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get results for electionId {electionId}", electionId);
            throw;
        }
    }
}