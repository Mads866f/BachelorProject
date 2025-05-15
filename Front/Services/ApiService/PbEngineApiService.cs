using System.Text;
using System.Text.Json;
using DTO.Models;
using Front.Components.ResultPage.CoherrentVoter;
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
            var response = await _client.GetAsync("/realElections");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<string>>();
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


    public async Task<string> DownloadCustomElection(Election election)
    {
        var electionId = election.Id;
        _logger.LogInformation("Downloading Custom election " + electionId);
        try
        {
            var url = "download/"+electionId;
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using var result = await response.Content.ReadAsStreamAsync();
                //Creating the file beforehand
                var path = "custom-elections/" + election.Name + "_custom.pb";
                File.WriteAllText(path,"");
                //Writing actual content to the file
                var file = new FileStream(path,FileMode.Create);
                await result.CopyToAsync(file);
                return path;
            }
            throw new  InternalServerErrorException("Internal Server Error - Downloading Custom election");
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }

    public async Task<Dictionary<string, float>> GetAvgSatisfactions(ElectionResult electionResult,List<int> sats)
    {
        _logger.LogInformation("Getting Avg Satisfaction");
        try
        {
            var json = JsonSerializer.Serialize(sats);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/analyze/avgSatisfaction/{electionResult.Id}",content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, float>>();
                return result ?? throw new Exception("Internal Server Error - GetAvgSatisfaction");
            }  
            else
            {
                var exception = new InternalServerErrorException("Internal Server Error - Getting AvgSatisfaction");
                _logger.LogError(exception,exception.Message);
                throw exception;
            } 
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error Retrieving Real Elections");
            throw;
        }}

    public async Task<Dictionary<Guid, Dictionary<string, float>>> GetAvgSatisfactionCoherentGroups(
        List<CoherrentVoter> coherrents, ElectionResult electionResult,List<int> sat)
    {
        _logger.LogInformation("Getting Avg Satisfaction - For Coherent Groups");
        var load = new {groups = coherrents , sats = sat};
        var json = JsonSerializer.Serialize(load);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await _client.PostAsync( $"{url}/analyze/CoherentGroups/{electionResult.Id}", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<Guid, Dictionary<string, float>>>();
                return result ?? new Dictionary<Guid, Dictionary<string, float>>();
            }
            var error = new InternalServerErrorException("Internal Server Error - GetAvgSatisfactionCoherentGroups");
            _logger.LogError(error,error.Message);
            throw error;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error Calculating Avg Satisfaction");
            throw;
        }

    }

    public async Task<ElectionResult> RedoElection(Election election)
    {
        _logger.LogInformation("Redo Election");
        var json = JsonSerializer.Serialize(election);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
           var response = await _client.PostAsync($"{url}/redoElection", content);
           if (response.IsSuccessStatusCode)
           {
               var result = await response.Content.ReadFromJsonAsync<ElectionResult>();
               return result ?? throw new Exception("Internal Server Error - RedoElection");
           }
           var error =  new InternalServerErrorException("Internal Server Error - RedoElection");
           _logger.LogError(error,error.Message);
           throw error;
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }
}