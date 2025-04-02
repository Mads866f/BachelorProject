using Backend.Models;
using Backend.Services.ApiServices.PbEngine;
using Backend.Services.Interfaces;
using Backend.Services.Interfaces.PbEngine;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class PbEngineController(IElectionService _electionService, IProjectService _projectService, IVotersService _votersService, IPbEngineService _service)
{

    [HttpGet("{id}")]
    public async Task<List<Project>> CalculateElection(string id)
    {
        var electionEntity = await _electionService.GetElectionAsync(id);
        var electionId = electionEntity is not null ? electionEntity.Id : Guid.Empty;
        var method = 1; //TODO CHANGE IN DB TO STORE AS CONSTANTS FOR BETTER COMMUNICATION WITH PBENGIN
        var ballot_design = 1; //TODO SAME AS ABOVE
        var voters = await _votersService.GetVotersByElectionId(electionId);
        var projects = await _projectService.GetProjectsWithElectionId(electionId);
        
        
        var pythonProjects = projects.Select(p => 
            new PythonProject(p.Id.ToString(),p.Cost,
                p.categories is not null? p.categories.Select(c => c.Name).ToList():[],
                p.targets is not null? p.targets.Select(t => t.Name).ToList():[])).ToList();

        var pythonVoters = voters.Select(vot => new PythonVoter()
        {
            SelectedProjects = vot.Votes.Select(v => v.Project_Id.ToString()).ToList(),
            SelectedDegree = vot.Votes.Select(v => v.Grade).ToList(),
        }).ToList();
        
        
        var electionPython = new PythonElection();
        electionPython.TotalBudget = electionEntity!.TotalBudget;
        electionPython.Votes = pythonVoters;
        electionPython.Projects = pythonProjects;
        
        
        
        

        var result = await _service.CalculateElection(electionPython,method,ballot_design);
        
        var result_PythonProjects_project = result.Select(p =>
            {
                var new_p = new Project
                {
                    Id = Guid.Parse(p.name),
                    ElectionId = electionId,
                    Name = projects.Where(proj => proj.Id.ToString() == p.name).First().Name,
                    Cost = p.cost,
                };
                return new_p;
            }).ToList();
            
        return result_PythonProjects_project;
    }
}