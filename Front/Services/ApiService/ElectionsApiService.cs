using DTO.Models;
using Front.Services.Elections;
using Front.Utilities;
using Front.Utilities.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Front.Services.ApiService.Elections;

public class ElectionsApiService (IHttpClientFactory clientFactory, ILogger<ElectionsApiService> _logger) : IElectionsApiService
{
   
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/Election";
    
   /// <summary>
   ///  Requests to create a new election within the database
   /// </summary>
   /// <param name="election">
   /// The Election To be created
   /// </param>
   /// <returns>
   /// Returns the Election if it is created
   /// </returns>
   /// <exception cref="CreationException"></exception>
   /// <exception cref="InternalServerErrorException"></exception>>
    public async Task<Election> CreateElection(Election election)
    {
        _logger.LogInformation("Creating Election with id: {id}",election.Id); 
        try
        {
            election.Id = Guid.NewGuid();//TODO CHECK IF THIS IS NEEDED
            var response = await _client.PostAsJsonAsync( url, election);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Election>();
                if (result is not null)
                {
                    return result;
                }
                else
                {
                    var exception = new CreationException("Failed to create Election");
                    _logger.LogError(exception, "Create Exception");
                    throw exception;
                }
            }

            else
            {
                _logger.LogError("Failed to create Election - Internal Server Error");
                throw new InternalServerErrorException("Internal server error - Create Election");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create Election");
            throw;
        } 
        
        
    }


/// <summary>
///  Requests all elections from the database through backend
/// </summary>
/// <returns>
/// A List of the elections.
/// </returns>
/// <exception cref="InternalServerErrorException"></exception>
    public async Task<List<Election>> GetElections() 
    {
        _logger.LogInformation("Getting Elections");
        try
        {

            var response = await _client.GetAsync(url);
            Console.WriteLine(("Got Response From Backend" +  response.StatusCode));
            if (response.IsSuccessStatusCode)
            {
                var elections = await response.Content.ReadFromJsonAsync<List<Election>>();
                return elections ?? new List<Election>();
            }
            else
            {
                _logger.LogError("Error getting Elections - Internal Server Error");
                throw new InternalServerErrorException("Error getting Elections from client");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting Elections");
            throw;
        }
    }
/// <summary>
///  Requests a specific election based on the Id from the database
/// </summary>
/// <param name="id">
///The Guid that represents the id of the election
/// </param>
/// <returns>
/// An Election if found, otherwise null
/// </returns>
/// <exception cref="InternalServerErrorException"></exception>
    public async Task<Election?> GetElection(Guid id)
    {
        _logger.LogInformation("Getting Election Called with id: " + id);
        try
        {
            var response = await _client.GetAsync(url+"/"+id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Election>();
                return result ?? throw new NotFoundError(($"Election Not Found With Id: " + id));
            }
            else
            {
                _logger.LogError("Error getting Election - Internal Server Error - Internal Server Error");
                throw new InternalServerErrorException(("Error getting Election with id: " + id));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting Election");
            throw;
        } 
    }


    public Task<Election> UpdateElection(Election election)
    {
        throw new NotImplementedException();
    }

    public Task DeleteElection(Guid id)
    {
        throw new NotImplementedException();
    }
}