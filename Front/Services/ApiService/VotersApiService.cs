using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using DTO.Models;
using Front.Components.ResultPage.CoherrentVoter;
using Front.Utilities;
using Front.Services.Interface;
using Front.Utilities.Errors;
using Microsoft.VisualBasic;
using MudBlazor;
using Constants = Front.Utilities.Constants;

namespace Front.Services.ApiService;

public class VotersApiService(IHttpClientFactory clientFactory, ILogger<VotersApiService> _logger) : IVotersApiService
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/voters";
    
    /// <summary>
    /// Requests to get specific voter with given id 
    /// </summary>
    /// <param name="id">
    /// Id of desired voter
    /// </param>
    /// <returns>
    /// The voter with the given id
    /// </returns>
    /// <exception cref="NotFoundError"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    public async Task<Voter> GetVoterById(Guid id)
    {
        _logger.LogInformation($"Getting voter by id {id}");
        try
        {
            var response = await _client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
               var result = await response.Content.ReadFromJsonAsync<Voter>();
               if (result is not null)
               {
                   return result;
               }
               else
               {
                   
                   var exception =  new NotFoundError("Voter not found with id: " + id);
                   _logger.LogError(exception, exception.Message);
                   throw exception;
               }

            }
            
            var ex = new InternalServerErrorException("Internal server error - GetVoterById");
            _logger.LogError(ex, "Internal server error - GetVoterById");
            throw ex;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error - GetVoterById");
            throw;
        }
    }
    /// <summary>
    ///  Requests all voters tied to a specific election
    /// </summary>
    /// <param name="electionId">
    /// The Id of the election
    /// </param>
    /// <returns>
    /// A list of the voters from the election
    /// </returns>
    /// <exception cref="NotFoundError"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    public async Task<List<Voter>> GetVotersByElectionId(Guid electionId)
    {
        // TODO NEED TO MODIFY CONTROLLER FOR EASIER ACCESS TO SPECIFIC VOTER GROUPS
        _logger.LogInformation($"Getting voters by election id {electionId}");
        try
        {
            var response = await _client.GetAsync($"{url}/election/{electionId}");
            if (response.IsSuccessStatusCode)
            {
                var result =  await response.Content.ReadFromJsonAsync<List<Voter>>();
                return result ?? [] ;
            }
            else
            {
                var exception = new InternalServerErrorException("Internal server error - GetVotersByElectionId");
                _logger.LogError(exception, exception.Message);
                throw exception;
            }
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting voters by election id");
            throw;
        }
    }
/// <summary>
///  Requests to create a new voter within the database
/// </summary>
/// <param name="electionId">
/// The Id of the election to which the voter belongs
/// </param>
/// <returns>
/// StatusCode Created(201) if created
/// </returns>
/// <exception cref="InternalServerErrorException"></exception>
    public async Task<int> CreateVoter(Guid electionId) //TODO CHANGE CHAIN TO VOTER INSTEAD OF SATUSCODE
    {
        _logger.LogInformation($"Creating voter by id {electionId}");
        try
        {
            var voter = new CreateVoter(){ElectionId = electionId};
            var response = await _client.PostAsJsonAsync(url,voter);
            if (response.IsSuccessStatusCode)
            {
                return StatusCodes.Status201Created;
            }
            else
            {
                var exception = new InternalServerErrorException("Internal server error - CreateVoter");
                _logger.LogError(exception, exception.Message);
                throw exception;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error - CreateVoter");
            throw;
        }
    }

    public async Task<List<CoherrentVoter>> GetCoherentVotersByElectionId(Guid electionId, int noOfProjectsInGroup, int lowerbound)
    {
        
        _logger.LogInformation($"Getting Coherent voters by election id {electionId},  no of projects in {noOfProjectsInGroup}, lowerbound {lowerbound}");
        try
        {
            var response = await _client.GetAsync($"{url}/{electionId}/{noOfProjectsInGroup}/{lowerbound}");
            if (response.IsSuccessStatusCode)
            {
                var result =  await response.Content.ReadFromJsonAsync<List<CoherrentVoter>>();
                return result ?? new List<CoherrentVoter>();
            }
            var exception = new InternalServerErrorException("Internal server error - GetCoherentVotersByElectionId");
            _logger.LogError(exception, exception.Message);
            throw exception;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}