using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.ApiServices.PbEngine;
using Backend.Services.DataServices;
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
    IPbEngineService _service,
    ElectionResultService _resultService,
    IScoresService _scoresService
    ) :Controller
{

    private async Task<PythonElection> createPythonElection(Election election)
    {
        var electionId=election.Id;
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
            totalBudget = election.TotalBudget,
            projects = pythonProjects,
            votes = pythonVoters,
            name = election.Name,
            method = election.Model,
            ballot_type = election.BallotDesign
        };
        return electionPython;
    }

    private Election election_from_electionEntity(ElectionEntity electionEntity)
    {
        return new Election()
        {
            Id = electionEntity.Id,
            Name = electionEntity.Name,
            BallotDesign = electionEntity.BallotDesign,
            Model = electionEntity.Model,
            TotalBudget = electionEntity.TotalBudget
        };
    }
    
    [HttpGet("{id}")]
    public async Task<List<Project>> CalculateElection(Guid id)
    {
        var electionEntity = await _electionService.GetElectionAsync(id);
        var electionId = electionEntity is not null ? electionEntity.Id : Guid.Empty;
        var method = 1; //TODO CHANGE IN DB TO STORE AS CONSTANTS FOR BETTER COMMUNICATION WITH PBENGIN
        var ballotDesign = 1; //TODO SAME AS ABOVE
        var projects = await _projectService.GetProjectsWithElectionId(electionId);
        
        var electionPython = await createPythonElection(electionEntity); 

            

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
        
        var electionResult = new ElectionResult()
            {ElectionId = id, ElectedProjects = result_PythonProjects_project, UsedBallot = ballotDesign.ToString(), UsedMethod = method.ToString(),  SubmittedProjects = projects.ToList()};
        await _resultService.AddElectionResult(electionResult); 
       
        return result_PythonProjects_project;
    }

    /// <summary>
    ///  Simple method for getting list of files of elections within "real-elections" folder
    /// </summary>
    /// <returns>
    /// List of file paths as strings
    /// </returns>
    [HttpGet("/realElections")]
    public List<string> GetRealElections()
    {
        Console.WriteLine("Real Elections Was Requested");
        var folder = "../real-elections";
        string[] files = Directory.GetFiles(folder);
        Console.WriteLine(files[0]);
        var result = files.ToList();
        return result;
    }

    [HttpGet("/realElections/{filename}")]
    public async Task<Election> GetRealElections(string filename)
    {
        var result = await _service.convert_real_election(filename);
        if (result is null)
        {
            Console.WriteLine(filename + " not found");
        } 
            
        //Adding the election to the database
        var election = new CreateElectionModel(){Name = result.name,TotalBudget = result.totalBudget,BallotDesign = result.ballot_type, Model = result.method};
        var election_created= await _electionService.CreateElectionAsync(election);
        //Adding the projects
        var projects = result.projects;
        var projectToIdMap = new Dictionary<string, Guid>();
        foreach (var pythonProject in projects)
        {
            var project = new CreateProjectModel()
            {
                ElectionId = election_created.Id,
                Name = pythonProject.name,
                Cost = pythonProject.cost,
                Categories = [],
                Targets = []
            };
            var projectCreated = await _projectService.CreateProjectAsync(project);
            projectToIdMap.Add(pythonProject.name, projectCreated.Id);
        }
        //Adding voters and votes
        var voters = result.votes;
        foreach (var pythonVoter in voters)
        {
            //Adding Voter
            var voter_model = new CreateVoter(){ElectionId = election_created.Id};
            var createdVoter = await _votersService.CreateVoterAsync(voter_model);
            //Adding votes
            for (int i = 0; i < pythonVoter.selectedProjects.Count; i++)
            {
                var project = pythonVoter.selectedProjects[i];
                var degree = pythonVoter.selectedDegree[i];
                //Adding Score to database
                var score = new Scores(){Grade = degree,  Voter_Id = createdVoter.Id, Project_Id = projectToIdMap[project]};
                await _scoresService.CreateVotersAsync(score);
            }
        }
        return (election_created);
        
    }

    [HttpGet("/download/{id}")]
    public async Task<IActionResult> DownloadElection(Guid id)
    {
        var election = await _electionService.GetElectionAsync(id);
        if (election is null)
        {
            Console.WriteLine(id + " not found");
            throw new Exception("Election not found");
        }
        var pythonElection = await createPythonElection(election);
        var fileStream = await _service.DownloadElection(pythonElection);
        if (fileStream is null)
        {
            Console.WriteLine(id + " not found");
            throw new Exception("Election not found");
        }
        

        return File(fileStream, "application/octet-stream",election.Name + "_custom.pb");
    }
}