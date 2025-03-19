using DTO.Models;
using Front.Services.Elections;
using Front.Utilities;

namespace Front.Services.ApiService.Elections;

public class ElectionsApiService (IHttpClientFactory clientFactory) : IElectionsApiService
{
   
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    
    
    public async Task<Election> CreateElection(Election election)
    {
        var url = "api/Election";
        Console.Write("Election create Called with election: " + election.name);
        try
        {
            election.id = Guid.NewGuid();
            election.JoinCode = Guid.NewGuid().ToString().Substring(0,8);
            var response = await _client.PostAsJsonAsync( url, election);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(("Got Response From Backend"));
                return await response.Content.ReadFromJsonAsync<Election>() ?? new Election
                {
                    name = "",
                    TotalBudget = 0,
                    model = "",
                    BallotDesign = ""
                };
            }
            
            return election;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Adding Instance \n"+e);
            throw;
        } 
        
        
    }
    
    
    
    public Task<List<Election>> GetElections()
    {
        throw new NotImplementedException();
    }

    public Task<Election> GetElection(int id)
    {
        throw new NotImplementedException();
    }


    public Task<Election> UpdateElection(Election election)
    {
        throw new NotImplementedException();
    }

    public Task DeleteElection(int id)
    {
        throw new NotImplementedException();
    }
}