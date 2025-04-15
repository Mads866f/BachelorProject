using DTO.Models;
using Front.Services.Interface;
using Front.Utilities;
using Front.Utilities.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Front.Services.ApiService;

public class PbEngineApiService(IHttpClientFactory clientFactory, ILogger<PbEngineApiService> _logger) : IPbEngineApiService
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/pbengine";

    /// <summary>
    /// Request the PbEngine to calculate the result of an election
    /// </summary>
    /// <param name="electionId">
    /// The Id Of the election which result should be calculated for.
    /// </param>
    /// <returns>
    /// List of projects, signifying which projects are chosen for the election.
    /// </returns>
    /// <exception cref="NotFoundError"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    public async Task<List<Project>> CalculateElection(Guid electionId)
    {
        _logger.LogInformation("Calculating election with id: "+ electionId);
        try
        {
            var response = await _client.GetAsync(url+"/"+electionId);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Project>>();
                if (result is not null)
                {
                    return result;
                }
                else
                {
                    var exception = new NotFoundError("Election with Id not found: " +  electionId);
                    _logger.LogError(exception,"Election Id Not Found: "+electionId + " - CaclulateElection");
                    throw exception;
                }
            }
            else
            {
                var exception = new InternalServerErrorException("Internal Server Error - Create Election");
                _logger.LogError(exception,"Election Id Not Found: "+electionId);
                throw exception;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error Calculating Election");
            throw;
        }
    }

    public async Task<List<string>> GetRealElectionsNames()
    {
        _logger.LogInformation("Getting real elections names");
        try
        {
            Console.WriteLine(url+"/realElections");
            var response = await _client.GetAsync("/realElections");
            Console.WriteLine("RESPONSE: " + response);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<string>>();
                Console.WriteLine(result);
                return result ?? new List<string>();
            }  
            else
            {
                var exception = new InternalServerErrorException("Internal Server Error - Getting real elections names");
                _logger.LogError(exception,exception.Message);
                throw exception;
            } 
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error Retrieving Real Elections");
            throw;
        }
    }

    public async Task<Election> DownloadRealElection(string nameOfElection)
    {
        _logger.LogInformation("Downloading real election " + nameOfElection);
        try
        {
           var response = await _client.GetAsync("/realElections/"+nameOfElection);
           if (response.IsSuccessStatusCode)
           {
               var result = await response.Content.ReadFromJsonAsync<Election>();
               return result;
           }
           else
           {
               var exception = new InternalServerErrorException("Internal Server Error - Downloading real election");
               _logger.LogError(exception,exception.Message);
               throw exception;
           } 
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error Calculating Election");
            throw;
        }
           
    }
}