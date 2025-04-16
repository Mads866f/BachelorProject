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
            Console.WriteLine(json);
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
}