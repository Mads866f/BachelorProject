using System.Reflection.Metadata;
using DTO.Models;
using Front.Utilities;
using Front.Services.Interface;
using Microsoft.VisualBasic;
using Constants = Front.Utilities.Constants;

namespace Front.Services.ApiService;

public class VotersApiService(IHttpClientFactory clientFactory) : IVotersApiService
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/voters";
    public async Task<Voter> GetVoter(string id)
    {
        Console.WriteLine("Get Voter (Frontend)");
        try
        {
            var response = await _client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
               var result = await response.Content.ReadFromJsonAsync<Voter>();
               return result ?? new Voter();
            }

            return new Voter();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error:GetVoter Frontend: " + e.ToString());
            throw;
        }
    }

    public Task<List<Voter>> GetVoters(string electionId)
    {
        throw new NotImplementedException();
    }
}