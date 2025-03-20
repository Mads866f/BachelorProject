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
        var election = new PythonElection()
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

        // Append method and ballot_type as query parameters
        var url = "/getResult/?method=equalShares&ballot_type=1-approval";

        try
        {
            var json = JsonSerializer.Serialize(election,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpsClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var projects = JsonSerializer.Deserialize<List<PythonProjects>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<PythonProjects>();

                // Print received projects
                projects.ForEach(x => Console.WriteLine($"Project: {x.name}, Cost: {x.cost}"));

                return projects;
            }

            Console.WriteLine($"Error: {response.StatusCode}");
            return new List<PythonProjects>();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            throw;
        }
    }
}