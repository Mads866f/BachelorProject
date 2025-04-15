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
            totalBudget = 10000,
            projects =
            [
                new PythonProject{name = "Project A", cost = 3000,categories = [],target = []},
                new PythonProject{name = "Project B", cost = 5000, categories = [],target = []}
            ],
            votes =
            [
                new PythonVoter{selectedProjects = ["Project B","Project A"], selectedDegree = [2,3]},
                new PythonVoter{selectedProjects = ["Project A","Project B"], selectedDegree = [1,2]}
            ]
        };

        // Append method and ballot_type as query parameters
        var url = "/getResult/?method=1&ballot_type=1";

        try
        {
            var json = JsonSerializer.Serialize(election,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpsClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("RESPONSE:\n" + jsonString);

                var projects = JsonSerializer.Deserialize<List<PythonProjects>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<PythonProjects>();

                // Print received projects
                projects.ForEach(x => Console.WriteLine($"Project: {x.Name}, Cost: {x.Cost}"));

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