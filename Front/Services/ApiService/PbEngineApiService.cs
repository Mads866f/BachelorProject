using DTO.Models;
using Front.Services.Interface;
using Front.Utilities;

namespace Front.Services.ApiService;

public class PbEngineApiService(IHttpClientFactory clientFactory) : IPbEngineApiService
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/pbengine";

    public async Task<List<Project>> CalculateElection(string electionId)
    {
        Console.WriteLine("Calculating election (Frontend)");
        try
        {
            var response = await _client.GetAsync(url+"/"+electionId);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<Project>>();
                return result ?? new List<Project>();
            }
            else
            {
                Console.WriteLine("Error in received Response Calculate Election (Frontend)");
                return new List<Project>();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}