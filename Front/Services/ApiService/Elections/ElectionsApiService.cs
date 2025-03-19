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



    public async Task<List<Election>> GetElections()
    {
        Console.WriteLine("Get Elections Method Called");
        const string url = "api/Election";
        try
        {

            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Got Response From Backend");
                var elections = await response.Content.ReadFromJsonAsync<List<Election>>();
                return elections ?? new List<Election>();
            }
            else
            {
                Console.WriteLine("Error in received response");
                return new List<Election>();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Got Some kind of error from getElections:");
            Console.WriteLine(e);
            throw;
        }
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