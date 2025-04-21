using DTO.Models;
using Front.Services.Interface.Elections;
using Front.Utilities;
using Front.Utilities.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Front.Services.ApiService.Elections;

public class ProjectsApiService(IHttpClientFactory clientFactory, ILogger<ProjectsApiService> _logger) : IProjectsApiService
{
    private readonly HttpClient _client = clientFactory.CreateClient(Constants.Backend);
    private readonly string url = "api/Project/";
    
   /// <summary>
   ///  Requests a list of projects from the database belonging to the election
   /// </summary>
   /// <param name="id">
   /// The Id of the election which the projects should belong
   /// </param>
   /// <returns>
   /// A list of projects
   /// </returns>
   /// <exception cref="InternalServerErrorException"></exception>
    public async Task<List<Project>> GetProjectsWithElectionId(Guid id)
    {
        _logger.LogInformation("Get Projects from election with Id: "+id);
        try
        {
            var response = await _client.GetAsync(url + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<Project>>();
                return content ?? [];
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("No projects found for election with Id: " + id);
                return [];
            }
            var exception = new InternalServerErrorException("Internal Server Error - GetProjectsWithElectionId");
            _logger.LogError(exception, "Internal Server Error - GetProjectsWithElectionId");
            throw exception;
        }
        catch (Exception e)
        {
            _logger.LogError(e ,"Error occured - GetProjectsWithElectionId");
            throw;
        }
    }

   /// <summary>
   ///  Requests the database to create a project
   /// </summary>
   /// <param name="projectToCreate"></param>
   /// <returns>
   /// Statuscode 201 if project is created
   /// </returns>
   /// <exception cref="InternalServerErrorException"></exception>
    public async Task<int> CreateProject(CreateProjectModel projectToCreate) //TODO DISCUSS IF RETURNTYPE IS CORRECT?
    {
        _logger.LogInformation("Create Project with name: " + projectToCreate.Name);
       try
       {
           var response = await _client.PostAsJsonAsync(url, projectToCreate);
           if (response.IsSuccessStatusCode)
           {
               _logger.LogInformation("Created Project with name: " + projectToCreate.Name + "successfully");
               return StatusCodes.Status201Created;
           }
           else
           {
                var exception = new InternalServerErrorException("Internal Server Error - CreateProject");
                _logger.LogError(exception, "Internal Server Error - CreateProject");
                throw exception;
           }
       }
       catch (Exception e)
       {
           _logger.LogError(e ,"Error occured - CreateProject");
           throw;
       }
    }
/// <summary>
///  Requests to update an entry within the database
/// </summary>
/// <param name="projectToUpdate">
/// The project which is requested to be updated
/// </param>
/// <returns>
/// Statuscode Ok (200) if updated
/// </returns>
/// <exception cref="InternalServerErrorException"></exception>
    public async Task<int> UpdateProject(Project projectToUpdate) //TODO DISCUSS RETURN TYPE
    {
        _logger.LogInformation("Update Project with name: " + projectToUpdate.Name);
        try
        {
            var response = await _client.PutAsJsonAsync(url, projectToUpdate);
            if (response.IsSuccessStatusCode)
            {
                return StatusCodes.Status200OK;
            }
            else
            {
                var exception = new InternalServerErrorException("Internal Server Error - UpdateProject");
                _logger.LogError(exception, "Internal Server Error - UpdateProject");
                throw exception;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e ,"Error occured - UpdateProject");
            throw;
        }
    }

/// <summary>
///  Requests to delete Project Entry From database
/// </summary>
/// <param name="projectToDelete">
/// The project which is requested to be deleted
/// </param>
/// <returns>
/// Statuscode Ok (200), if deletion were successful
/// </returns>
/// <exception cref="InternalServerErrorException"></exception>
    public async Task<int> DeleteProject(Project projectToDelete) //TODO DISCUSS THIS RETURN TYPE AS WELL
    {
        _logger.LogInformation("Delete Project with name: " + projectToDelete.Name);
        try
        {
            var response = await _client.DeleteAsync(url + projectToDelete.Id);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Deleted Project with name: " + projectToDelete.Name + "successfully");
                return StatusCodes.Status200OK;
            }
            else
            {
                var exception = new InternalServerErrorException("Internal Server Error - DeleteProject");
                _logger.LogError(exception, "Internal Server Error - DeleteProject");
                throw exception;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e ,"Error occured - DeleteProject");
            throw;
        }
    }
}