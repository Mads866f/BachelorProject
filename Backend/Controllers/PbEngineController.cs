using Backend.Models;
using Backend.Services.ApiServices.PbEngine;
using Backend.Services.Interfaces;
using Backend.Services.Interfaces.PbEngine;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/pbengine")]
public class PbEngineController(IElectionService _electionService,
    IProjectService _projectService,
    IVotersService _votersService,
    IPbEngineService _service)
{

    [HttpGet("{id}")]
    public async Task<List<Project>> CalculateElection(Guid id)
    {
        var electionEntity = await _electionService.GetElectionAsync(id);
        var electionId = electionEntity is not null ? electionEntity.Id : Guid.Empty;
        var method = 1; //TODO CHANGE IN DB TO STORE AS CONSTANTS FOR BETTER COMMUNICATION WITH PBENGIN
        var ballotDesign = 1; //TODO SAME AS ABOVE
        var voters = await _votersService.GetVotersByElectionId(electionId);
        var projects = await _projectService.GetProjectsWithElectionId(electionId);
        
        
        var pythonProjects = projects.Select(p => 
            new PythonProject(p.Id.ToString(),p.Cost,
                p.Categories is not null? p.Categories.Select(c => c.Name).ToList():new List<string>(),
                p.Targets is not null? p.Targets.Select(t => t.Name).ToList():new List<string>())).ToList();

        var pythonVoters = voters.Select(vot => new PythonVoter()
        {
            selectedProjects = vot.Votes.Select(v => v.Project_Id.ToString()).ToList(),
            selectedDegree = vot.Votes.Select(v => v.Grade).ToList(),
        }).ToList();
        
        
        var electionPython = new PythonElection
        {
            totalBudget = electionEntity!.TotalBudget,
            projects = pythonProjects,
            votes = pythonVoters
        };

            

        var result = await _service.CalculateElection(electionPython,method,ballotDesign);
        
        var result_PythonProjects_project = result.Select(p =>
            {
                var new_p = new Project
                {
                    Id = Guid.Parse(p.Name),
                    ElectionId = electionId,
                    Name = projects.Where(proj => proj.Id.ToString() == p.Name).First().Name,
                    Cost = p.Cost,
                };
                return new_p;
            }).ToList();
            
        return result_PythonProjects_project;
    }
}