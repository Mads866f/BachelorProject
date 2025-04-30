using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using Backend.Models;
using Backend.Services.Interfaces.PbEngine;
using Backend.Utilities;
using DTO.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Backend.Services.ApiServices.PbEngine;

public class PbEngineService(IHttpClientFactory clientFactory) : IPbEngineService
{
    private readonly HttpClient _httpsClient = clientFactory.CreateClient(Constants.PbEngine);
        
    public async Task<List<PythonProjects>> CalculateElection(PythonElection election,int method,int ballotType)
    {
        var url  = "/getResult/?method="+method+"&ballot_type="+ballotType;
        try
        {
            var json = JsonSerializer.Serialize(election);
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

    public async Task<PythonElection?> convert_real_election(string fileName)
    {
        var url = "realElections?file_name="+fileName;
        try
        {
            var response = await _httpsClient.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<PythonElection>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Stream> DownloadElection(PythonElection election)
    {
        var url = "downloads/";
        try
        {
            var json = JsonSerializer.Serialize(election);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpsClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
                /*using var result = await response.Content.ReadAsStreamAsync();
                //Creating the file beforehand
                var path = "custom-elections/" + election.name + "_custom.pb";
                File.WriteAllText(path,"");
                //Writing actual content to the file
                var file = new FileStream(path,FileMode.Create);
                await result.CopyToAsync(file);
                return file;
                */
            }
            throw new Exception("Internal Server Error");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Dictionary<string, float>> GetAnalysisNumbers(PythonElection election,List<PythonProject> electedProjects, List<int> sats)
    {
        var url = "analyze/";
        Console.WriteLine($"BUDGET BACKEND: {election.totalBudget}");
        Console.WriteLine($"NUMBER OF ELECTED PROJECTS: {electedProjects.Count()}");
        var load = new
        {
            election = election,
            outcome = electedProjects,
            satisfactions = sats
        };
        var json = JsonSerializer.Serialize(load);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpsClient.PostAsync(url + "avgSatisfaction", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                
                var map = new Dictionary<string, float>();
                using var doc = JsonDocument.Parse(jsonString);


                if (doc.RootElement.TryGetProperty("result", out var resultElement))
                {
                    foreach (var prop in resultElement.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == JsonValueKind.Number
                            && prop.Value.TryGetSingle(out var f))
                        {
                            map[prop.Name] = f;
                        }
                    }
                }

                return map;
            }

            throw new Exception("Internal Server Error");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Dictionary<PythonVoter, Dictionary<string, float>>> GetAnalysisNumbersGroups(PythonElection pythonElection, List<PythonProject> pythonProjectElected, List<int> sats)
    {
        var url = "analyze/";
        var load = new
        {
            election = pythonElection,
            outcome = pythonProjectElected,
            satisfactions = sats
        };
        var json = JsonSerializer.Serialize(load);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpsClient.PostAsync(url + "avgSatisfaction/groups", content);
            if (response.IsSuccessStatusCode)
            {
                // 1) Read as string-keyed dictionary
                var raw = await response.Content
                    .ReadFromJsonAsync<Dictionary<string, Dictionary<string, Dictionary<string, float>>>>();
                if (raw == null)
                    throw new Exception("Internal Server Error – GetAnalysisNumbersGroups");
                
                var actualContent = raw["result"];
                var satisfactionsInDict= actualContent.Keys.ToList();
                var groups = actualContent.Values.First().Keys.ToList();
                var tmpVoter = new Dictionary<string, Dictionary<string, float>>();
                groups.ForEach(g => tmpVoter.Add(g, new Dictionary<string, float>()));
                foreach (var sat in satisfactionsInDict)
                {
                    var satBook = actualContent[sat];
                    groups.ForEach(g => tmpVoter[g][sat]=satBook[g]);
                }

                var result = new Dictionary<PythonVoter, Dictionary<string, float>>(); 
                foreach (var dict in tmpVoter)
                {
                    var projectNames = dict.Key.Split("***").ToList();
                    projectNames.ForEach(p => p.Replace("***",""));
                    projectNames.Remove(projectNames.Last());
                    var toPythonVoter = new PythonVoter()
                    {
                        selectedProjects = projectNames,
                        selectedDegree = projectNames.Select(_=>1).ToList()
                    };
                    result.Add(toPythonVoter, dict.Value);
                }
                return result;
            }

            throw new Exception("Internal Server Error");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}