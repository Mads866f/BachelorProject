using System.Text;
using System.Text.Json;
using Backend.Models;
using Backend.Services.Interfaces.PbEngine;
using Backend.Utilities;

namespace Backend.Services.ApiServices.PbEngine;

public class Initialtest(IHttpClientFactory clientFactory) : IInitialtest
{
    private readonly HttpClient _httpsClient = clientFactory.CreateClient(Constants.PbEngine);

    public async Task<string> Test()
    {
        var url = "/test";
        try
        {
            var response = await _httpsClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return "";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<PythonProjects>> TestElection()
    {
        PythonElection Election = new PythonElection()
        {
            TotalBudget = 10000,
            Projects =
            [
                new KeyValuePair<string, int>("Project A", 3000),
                new KeyValuePair<string, int>("Project B", 5000)
            ],
            Votes =
            [
                new List<KeyValuePair<string, int>>
                {
                    new("Project A", 3),
                    new("Project B", 2)
                },

                new List<KeyValuePair<string, int>>
                {
                    new("Project A", 5),
                    new("Project B", 1)
                }
            ]
        };

        var url = "/getResult/";
        try
        {
            var requestBody = new
            {
                Method = "equalShares",  // Change this to your actual method value
                Ballot_type = "1-approval", // Change this to your actual ballot type
                Body = Election
            };

            var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpsClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<PythonProjects>>(jsonString) ?? new List<PythonProjects>();
            }

            return new List<PythonProjects>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}