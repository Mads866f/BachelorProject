using DTO.Models;
using Front.Services.Interface;
using Front.Utilities;

namespace Front.Services.ApiService;

public class ElectionResultsApiService(
    IHttpClientFactory clientFactory,
    ILogger<ElectionResultsApiService> logger
    ) : IElectionResultsApiService
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private const string Url = "api/ElectionResult/";

    /// <inheritdoc/>"/>
    public async Task<List<ElectionResult>?> GetResultsByElectionId(Guid electionId)
    {
        logger.LogInformation(
            "Fetching list of election results" +
            " for election with ID: {electionId}", electionId);
        try
        {
            var response = await _client.GetAsync(Url + electionId);
            if (!response.IsSuccessStatusCode) return null;
            var result = await response.Content.ReadFromJsonAsync<List<ElectionResult>>();
            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to fetch election results");
            throw;
        }
    }
}