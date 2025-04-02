using DTO.Models;
using Front.Services.Interface;
using Microsoft.VisualBasic;
using Constants = Front.Utilities.Constants;

namespace Front.Services.ApiService;

public class ScoresApiService(IHttpClientFactory clientFactory) : IScoresApiService
{
    
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/scores";
    
    public async Task UpdateScores(string voterId, Dictionary<string, int> votes)
    {
        Console.WriteLine("Updating scores (Frontend)");
        Console.WriteLine("VoterId: " + voterId);
        try
        {
            var response = await _client.PostAsJsonAsync(url+ "/"+voterId , votes);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully updated scores (Frontend)");
            }
            else
            {
                Console.WriteLine("Failed to update scores (Frontend)");
            } 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}