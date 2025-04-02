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

    public async Task<List<Voter>> GetVoters(string electionId)
    {
        // TODO NEED TO MODIFY CONTROLLER FOR EASIER ACCESS TO SPECIFIC VOTER GROUPS
        Console.WriteLine("Get Voters From Election (Frontend)");
        try
        {
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Get Voters From Election success (Frontend)");
                var result =  await response.Content.ReadFromJsonAsync<List<Voter>>();
                if (result != null) return result.Where(voter => voter.ElectionId.ToString() == electionId).ToList();
            }
            else
            {
                Console.WriteLine("Get Voters From Election Failed(Frontend)");
                return new List<Voter>();
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return new List<Voter>();
    }

    public async Task CreateVoter(string electionId)
    {
        Console.WriteLine("Create Voter (Frontend)");
        try
        {
            var voter = new CreateVoter(){ElectionId = Guid.Parse(electionId)};
            var response = await _client.PostAsJsonAsync(url,voter);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Voter Created SUCCESS");
            }
            else
            {
                Console.WriteLine("Voter Created FAILED");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}