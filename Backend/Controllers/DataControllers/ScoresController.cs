using System.Runtime.InteropServices;
using Backend.Models;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/scores")]
public class ScoresController(IScoresService service, ILogger<ScoresController> _logger) : ControllerBase
{
    private readonly IScoresService _service = service;
    
    /// <summary>
    ///  Request To update the scores of a specific voter within the database
    /// </summary>
    /// <param name="voterId">
    /// The Id of the voter voting 
    /// </param>
    /// <param name="scores">
    /// The different projects voted for and their "degree of approval"
    /// </param>
    /// <returns>
    /// Ok if update is successfull
    /// NotFound if voter is not found
    /// </returns>
    [HttpPost("{voterId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateScores(Guid voterId, [FromBody] Dictionary<string, int> scores)
    {
        try
        {
            _logger.LogInformation("Updating Scores for voter with id: {id})", voterId);
            //Get a set of the projects Id
            var projectsId = scores.Keys.ToList();

            //Remove previous Votes
            var prevVotes = await _service.GetScoresForVoterIdAsync(voterId);
            if (prevVotes is null)
            {
                return NotFound("No Voter");
            }
            foreach (var score in prevVotes)
            {
                var deletion = await _service.DeleteByIdAsync(voterId, score.Project_Id);
                if (deletion) continue;
                //Deletion were unsuccessful 
                _logger.LogError("Failed to delete Scores for voter with id: {id}", voterId);
                return StatusCode(500, ("No deletion found for voter with id: {voterId} and projectId {score.Project_Id}", voterId, score.Project_Id));
            }

            //Add new votes
            foreach (var project in projectsId)
            {
                var score = new Scores()
                    { Grade = scores[project], Voter_Id = voterId, Project_Id = Guid.ParseExact(project, "D") };
                var created = await _service.CreateVotersAsync(score);
                var createdSuccess = created is not null;//created is not null;
                if (!createdSuccess)
                {
                    //Creation were unsuccessful
                    _logger.LogError("Failed to create score with id: {id}, projectId: {project}", voterId,project);
                    return StatusCode(500,"Problem creating score");
                }
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Scores");
            return StatusCode(500);
        }
    }
}