using Backend.Models;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/[controller]")]
public class ElectionController(IElectionService service, ILogger<ElectionController> logger) : ControllerBase
{
        /// <summary>
        /// HttpGet call for getting all elections in the database 
        /// </summary>
        /// <returns>
        /// Ok with a list of elections
        /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Election>>> GetAll()
    {
        logger.LogInformation("Getting all Elections");
        try
        {
            // Await the result from the service call to database
            var result = await service.GetAllElectionsAsync();
            //Return Result
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while retrieving all Elections");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves a specific election with the given Id.
    /// </summary>
    /// <param name="id"> The Id of the election</param>
    /// <returns>Ok with the specific Election if found.
    /// Otherwise NotFound</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Election>> GetById(Guid id)
    {
        logger.LogInformation("Getting Election by id {id}",id);
        try
        {
            //Await the database for response
            var result = await service.GetElectionAsync(id);
            //Check if result signifies that the election exists within the database
            var resultExists = result is not null;
            //Returns the result or Notfound depending on bool
            return resultExists ? Ok(result) : NotFound();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while getting Election by {id}",id);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Requests to create A new Election in the database
    /// </summary>
    /// <param name="election">The Election To create</param>
    /// <returns>
    /// The Election Created in the database
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Election>> CreateElection([FromBody] CreateElectionModel election)
    {
        logger.LogInformation("Creating Election with name: {Name}",election.Name);
        try
        {
            //Await the database creation of the election
            Election createdElection = await service.CreateElectionAsync(election);
            //Returns the election - Note returns Ok(createdElection) Due to Asp .NET rules
            return createdElection;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while creating Election with name: {name}",election.Name);
            return StatusCode(500);
        }
    }

    
    /// <summary>
    /// Requests to update an entry of an election in the database
    /// </summary>
    /// <param name="election">
    /// The Election to be updated
    /// </param>
    /// <returns>
    /// The updated Election if update were successfully
    /// NotFound if update were unsuccessful
    /// </returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Election>> UpdateElection([FromBody] Election election)
    {
        logger.LogInformation("Updating Election with id: {id}",election.Id);
        try
        {
            //Await for database to update election
            var result = await service.UpdateElectionAsync(election);
            //Check if result exists
            var resultExists = result is not null;
            //Return ok or NotFound depending on bool
            return resultExists ? result : NotFound();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while updating Election with id: {id}",election.Id);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Requests to Delete an Election with the given Id from the database
    /// </summary>
    /// <param name="id">
    /// The Id Of the election to be deleted
    /// </param>
    /// <returns>
    /// Ok if election were deleted
    /// NotFound if election were unable to delete or no election existed with given id
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteByIdAsync(Guid id)
    {
        logger.LogInformation("Deleting Election with id: {id}",id);
        try
        {
            //Await for database to express if deletion of election were succesfull
            var result = await service.DeleteByIdAsync(id);
            //Return Ok or Notfound based on bool
            return result ? Ok() : NotFound();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while deleting Election with id: {id}",id);
            return StatusCode(500);
        }
    }
}