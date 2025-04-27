using Backend.Services.Interfaces;
using DTO.Models;
using Front.Components.ResultPage.CoherrentVoter;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotersController(IVotersService voterService, ILogger<VotersController> logger, IProjectService projectService)
        : ControllerBase
    {
        private readonly IVotersService _voterService = voterService ?? throw new ArgumentNullException(nameof(voterService));
        private readonly ILogger<VotersController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IProjectService _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));

        /// <summary>
        /// Retrieves all voters.
        /// </summary>
        /// <returns>A list of <see cref="Voter"/>s.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Voter>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Voter>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all voters.");

            try
            {
                var result = await _voterService.GetAllVotersAsync();
                var voterList = result.ToList();
                if (voterList.Count != 0)
                {
                    _logger.LogInformation("Found {VoterCount} voters.", voterList.Count);
                    foreach (var voter in voterList)
                    {
                        foreach (var score in voter.Votes)
                        {
                            var project = await _projectService.GetProjectByIdAsync(score.Project_Id);
                            score.project = project;
                        }
                    }
                    return Ok(result);
                }

                _logger.LogWarning("No voters found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all voters.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("election/{Electionid}")]
        public async Task<ActionResult<Voter>> GetByElectionId(Guid Electionid)
        {
            
            _logger.LogInformation($"Fetching all voters from election with id {Electionid}.");

            try
            {
                var result = await _voterService.GetVotersByElectionId(Electionid);
                var voterList = result.ToList();
                if (voterList.Count != 0)
                {
                    _logger.LogInformation("Found {VoterCount} voters.", voterList.Count);
                    foreach (var voter in voterList)
                    {
                        foreach (var score in voter.Votes)
                        {
                            var project = await _projectService.GetProjectByIdAsync(score.Project_Id); //TODO - Optimize this
                            score.project = project;
                        }
                    }
                    return Ok(result);
                }

                _logger.LogWarning("No voters found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all voters by Id.");
                return StatusCode(500, "Internal server error");
            }
        }
        
        /// <summary>
        /// Retrieves a voter by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the voter.</param>
        /// <returns>A single <see cref="Voter"/> if found.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Voter), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Voter>> GetAsync(Guid id)
        {
            _logger.LogInformation("Fetching voter with ID {VoterId}.", id);

            try
            {
                var result = await _voterService.GetVoterAsync(id);
                if (result is not null)
                {
                    return Ok(result);
                }

                _logger.LogWarning("Voter with ID {VoterId} not found.", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching voter with ID {VoterId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new voter.
        /// </summary>
        /// <param name="voter">The voter details to create.</param>
        /// <returns>The created <see cref="Voter"/> object.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Voter), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Voter>> Create([FromBody] CreateVoter voter)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state when attempting to create a voter.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating voter with electionId {electionId}.", voter.ElectionId);

            try
            {
                var createdVoter = await _voterService.CreateVoterAsync(voter);
                _logger.LogInformation("Voter created successfully with ID {VoterId}.", createdVoter.Id);
                return Ok(createdVoter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating voter electionId {electionId}.", voter.ElectionId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing voter.
        /// </summary>
        /// <param name="voter">The updated voter details.</param>
        /// <returns>The updated <see cref="Voter"/> object.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(Voter), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Voter>> Update([FromBody] Voter voter)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state when attempting to update voter with ID {VoterId}.", voter.Id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating voter with ID {VoterId}.", voter.Id);

            try
            {
                var result = await _voterService.UpdateVoterAsync(voter);
                if (result is not null)
                {
                    _logger.LogInformation("Voter with ID {VoterId} updated successfully.", voter.Id);
                    return Ok(result);
                }

                _logger.LogWarning("Voter with ID {VoterId} not found.", voter.Id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating voter with ID {VoterId}.", voter.Id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a voter by ID.
        /// </summary>
        /// <param name="id">The ID of the voter to delete.</param>
        /// <returns>A status indicating the success or failure of the operation.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            _logger.LogInformation("Deleting voter with ID {VoterId}.", id);

            try
            {
                var result = await _voterService.DeleteByIdAsync(id);
                if (result)
                {
                    _logger.LogInformation("Voter with ID {VoterId} deleted successfully.", id);
                    return Ok();
                }

                _logger.LogWarning("Voter with ID {VoterId} not found.", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting voter with ID {VoterId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{electionId}/{noOfProjectsInGroup}/{lowerbound}")]
        public async Task<IEnumerable<CoherrentVoter>> GetCoherentVotersFromElection(Guid electionId,
            int noOfProjectsInGroup, int lowerbound)
        {
            _logger.LogInformation($"Getting Coherent voters from election with id {electionId} of size {noOfProjectsInGroup}.}}");
            try
            {
                var result = await _voterService.GetCoherentVotersFromElection(electionId, noOfProjectsInGroup,lowerbound);
                return result ?? Enumerable.Empty<CoherrentVoter>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
