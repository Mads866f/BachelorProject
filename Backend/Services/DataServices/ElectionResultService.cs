using AutoMapper;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;

/// <inheritdoc />
public class ElectionResultService(IMapper mapper,
    IElectionResultRepository repository,
    ILogger<ElectionResultService> logger,
    IProjectService projectService) : IElectionResultService
{
    /// <inheritdoc />
    public async Task<List<ElectionResult>> GetElectionsResultsByElectionId(Guid electionId)
    {
        var electionResults = await repository.GetElectionsResultByElectionId(electionId);
        var result = new List<ElectionResult>();
        foreach (var electionResult in electionResults)
        {
           var electionres = new ElectionResult
           {
               Id = electionResult.Id,
               ElectionId = electionResult.ElectionId
           };
           logger.LogInformation("Constructing ElectionResult Dto with id: "+ electionres.Id + " Election Id: "+ electionResult.ElectionId);
           electionres.UsedBallot = electionResult.BallotUsed;
           electionres.UsedMethod = electionres.UsedMethod;
           var projects = await repository.GetProjectsByResultId(electionResult.Id);
           var projectsTransformed = projects.Select(p => mapper.Map<Project>(p)).ToList();
           electionres.ElectedProjects = projectsTransformed;
           //Getting Submitted Projects
           var projectsElected = await projectService.GetProjectsWithElectionId(electionResult.ElectionId);
           electionres.SubmittedProjects = projectsElected.ToList();
           result.Add(electionres);
        }
        return result;
    }
    
    /// <inheritdoc />
    public async Task<ElectionResult> AddElectionResult(ElectionResult result)
    {
        logger.LogInformation("Adding new election result with ElectionId: " + result.ElectionId);
        var resultFromDb = await repository.AddElectionResult(result);
        return resultFromDb;
    } 
}