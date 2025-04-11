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
    private const string Url = "api/ElectionResultController/";

    /// <inheritdoc/>"/>
    public async Task<List<ElectionResult>?> GetResultsByElectionId(Guid electionId)
    {
        logger.LogInformation(
            "Fetching list of election results" +
            " for election with ID: {electionId}", electionId);
        try
        {
            //TODO DUMMY RESPONSE
            var dummyElectionId = Guid.NewGuid();
            return
            [
                new ElectionResult()
                {
                    Id = Guid.NewGuid(),
                    ElectionId = dummyElectionId,
                    SubmittedProjects = [
                    new Project
                    {
                        ElectionId = dummyElectionId,
                        Name = "Project 1",
                        Cost = 100
                    },
                    new Project
                    {
                        ElectionId = dummyElectionId,
                        Name = "Project 2",
                        Cost = 200
                    }
                    ],
                    ElectedProjects = [
                    new Project
                    {
                        ElectionId = dummyElectionId,
                        Name = "Project 1",
                        Cost = 100
                    }
                    ],
                    UsedBallot = "",
                    UsedMethod = ""
                }
            ];
            // TODO end of dummy
            
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