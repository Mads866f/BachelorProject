using DTO.Models;
using Front.Services.Interface;
using Front.Utilities.Errors;
using Microsoft.VisualBasic;
using Constants = Front.Utilities.Constants;

namespace Front.Services.ApiService;

public class ScoresApiService(IHttpClientFactory clientFactory, ILogger<ScoresApiService> _logger) : IScoresApiService
{
    
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/scores";
    
    public async Task<int> UpdateScores(Guid voterId, Dictionary<string, int> votes)
    {
        _logger.LogInformation("UpdateScores for voter: " +  voterId);
        try
        {
            var response = await _client.PostAsJsonAsync(url+ "/"+voterId , votes);
            if (response.IsSuccessStatusCode)
            {
                return StatusCodes.Status200OK;
            }
            else
            {
                var exception = new InternalServerErrorException("Internal server error - UpdateScores");
                _logger.LogError(exception, "Internal server error - UpdateScores");
                throw exception;
            } 
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating scores");
            throw;
        }
    }
}