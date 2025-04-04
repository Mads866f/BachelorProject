using DTO.Models;
using Front.Services.Elections;
using Front.Services.Interface.Elections;
using Front.Utilities;

namespace Front.Services.ApiService.Elections;

public class ProjectsApiService(IHttpClientFactory clientFactory) : IProjectsApiService
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/Project/";
    
    
    public async Task<List<Project>> GetProjectsWithElectionId(string id)
    {
        Console.WriteLine("Getting Projects - Frontend");
        try
        {
            var response = await _client.GetAsync(url + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<Project>>();
                return content ?? new List<Project>();
            }
            else
            {
                Console.WriteLine("Error in received Response");
                return new List<Project>();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error Getting Projects For Election:"+e);
            throw;
        }
    }

    public async Task CreateProject(CreateProjectModel projectToCreate)
    {
       Console.WriteLine("Creating Project - Frontend");
       try
       {
           var response = await _client.PostAsJsonAsync(url, projectToCreate);
           if (response.IsSuccessStatusCode)
           {
               Console.WriteLine("Created Project Success!");
           }
           else
           {
               Console.WriteLine($"Error in received Response: {response.ReasonPhrase}");
           }
       }
       catch (Exception e)
       {
           Console.WriteLine(e);
           throw;
       }
    }

    public async Task UpdateProject(Project projectToUpdate)
    {
        Console.WriteLine("Updating Project - Frontend");
        try
        {
            var response = await _client.PutAsJsonAsync(url, projectToUpdate);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Updated Project Success!");
            }
            else
            {
                Console.WriteLine("Error in received Response");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR IN UPDATE:",e);
        }
    }

    public async Task DeleteProject(Project projectToDelete)
    {
        Console.WriteLine("Deleting Project - Frontend");
        try
        {
            var response = await _client.DeleteAsync(url + projectToDelete.Id);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Deleted Project Success!");
            }
            else
            {
                Console.WriteLine("Error in Deltion");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}