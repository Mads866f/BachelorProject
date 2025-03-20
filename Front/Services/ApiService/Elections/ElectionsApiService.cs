using DTO.Models;
using Front.Services.Elections;
using Front.Utilities;

namespace Front.Services.ApiService.Elections;

public class ElectionsApiService (IHttpClientFactory clientFactory) : IElectionsApiService
{
   
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/Election";
    
    
    public async Task<Election> CreateElection(Election election)
    {
        Console.Write("Election create Called with election: " + election.name);
        try
        {
            election.id = Guid.NewGuid();
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

    public async Task<Election> GetElection(string id)
    {
        try
        {
            var response = await _client.GetAsync(url+"/"+id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Election>();
                return result ?? new Election
                {
                    name = "asdas",
                    TotalBudget = 0,
                    model = "",
                    BallotDesign = ""
                };
            }

            return new Election
            {
                id = null,
                name = "nasda",
                TotalBudget = 0,
                model = "null",
                BallotDesign = "null"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        } 
    }


    public Task<Election> UpdateElection(Election election)
    {
        throw new NotImplementedException();
    }

    public Task DeleteElection(string id)
    {
        throw new NotImplementedException();
    }
}