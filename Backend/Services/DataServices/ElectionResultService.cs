using AutoMapper;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;

public class ElectionResultService(IMapper mapper, IElectionResultRepository repository, ILogger<ElectionResultService> _logger, IProjectService _projectService)
{
    
    public async Task<List<ElectionResult>> GetElectionsResultsByElectionId(Guid electionId)
    {
        var ElectionResults = await repository.GetElectionsResultByElectionId(electionId);
        var result = new List<ElectionResult>();
        foreach (var electionResult in ElectionResults)
        {
           var electionres = new ElectionResult();
           electionres.ElectionId = electionResult.ElectionId;
           electionres.UsedBallot = electionResult.BallotUsed;
           electionres.UsedMethod = electionres.UsedMethod;
           var projects = await repository.GetProjectsByResultId(electionResult.ElectionId);
           var projectsTransformed = projects.Select(p => mapper.Map<Project>(p)).ToList();
           electionres.ElectedProjects = projectsTransformed;
           var projectsElected = await _projectService.GetProjectsWithElectionId(electionResult.ElectionId);
           electionres.SubmittedProjects = projectsElected.ToList();
           result.Add(electionres);
        }
        return result;
    }


    public async Task<ElectionResult> AddElectionResult(ElectionResult result)
    {
        _logger.LogInformation("Adding new election result with ElectionId: " + result.ElectionId);
        var resultFromDb = await repository.AddElectionResult(result);
        return resultFromDb;
    } 
}