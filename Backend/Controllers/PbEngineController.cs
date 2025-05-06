using System.Collections;
using System.Runtime.InteropServices;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.ApiServices.PbEngine;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using Backend.Services.Interfaces.PbEngine;
using Backend.Utilities;
using DTO.Models;
using Front.Components.ResultPage.CoherrentVoter;
using Microsoft.AspNetCore.Components.Server.Circuits;
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
) : Controller
{

    public class groupssatDto()
    {
        public List<CoherrentVoter> groups { get; set; }
        public List<int> sats { get; set; }
    }

    private async Task<PythonElection> createPythonElection(Election election)
    {
        _electionService.EndElectionAsync(election.Id);
        var electionId = election.Id;
        var voters = await _votersService.GetVotersByElectionId(electionId);
        var projects = await _projectService.GetProjectsWithElectionId(electionId);


        var pythonProjects = projects.Select(p =>
            new PythonProject(p.Id.ToString(), p.Cost,
                p.Categories is not null ? p.Categories.Select(c => c.Name).ToList() : new List<string>(),
                p.Targets is not null ? p.Targets.Select(t => t.Name).ToList() : new List<string>())).ToList();

        var pythonVoters = voters.Select(vot => new PythonVoter()
        {
            selectedProjects = vot.Votes.Select(v => v.ProjectId.ToString()).ToList(),
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
        var method = Constants.rules_map[electionEntity!.Model];
        var ballotDesign = 1; //Hardcoded for approval - more ballots have not been implemented
        var projects = await _projectService.GetProjectsWithElectionId(electionId);

        var electionPython = await createPythonElection(electionEntity);



        var resultPythonProjectsProject = await CalculateHelperResult(electionPython, method, ballotDesign, electionId, projects);

        var electionResult = new ElectionResult()
        {
            ElectionId = id, ElectedProjects = resultPythonProjectsProject, UsedBallot = ballotDesign.ToString(),
            UsedMethod = Constants.rules_map.First(k => k.Value == method).Key, SubmittedProjects = projects.ToList(), TotalBudget = electionEntity.TotalBudget
        };
        await _resultService.AddElectionResult(electionResult);

        return resultPythonProjectsProject;
    }

    private async Task<List<Project>> CalculateHelperResult(PythonElection electionPython, int method, int ballotDesign, Guid electionId,
        IEnumerable<Project> projects)
    {
        var result = await _service.CalculateElection(electionPython, method, ballotDesign);

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

    [HttpPost("redoElection")]
    public async Task<ElectionResult> RedoElection([FromBody] Election modifiedElection)
    {
        var pythonElection = await createPythonElection(modifiedElection);
        var method = Constants.rules_map[modifiedElection.Model];
        var ballotDesign = 1; // Hardcoded for approval
        var electionId = modifiedElection.Id;
        var projects = await _projectService.GetProjectsWithElectionId(electionId);
        projects = projects.ToList();
        var electedProjects = await CalculateHelperResult(pythonElection, method, ballotDesign, electionId, projects);
        var electionResult = new ElectionResult()
        {
            ElectionId = electionId, ElectedProjects = electedProjects, UsedBallot = ballotDesign.ToString(),
            UsedMethod = Constants.rules_map.First(k => k.Value == method).Key, SubmittedProjects = projects.ToList(), TotalBudget = modifiedElection.TotalBudget
        };
        await _resultService.AddElectionResult(electionResult); 
        return electionResult;
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
        var folder = "real-elections";
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
        var election = new CreateElectionModel()
        {
            Name = result.name, TotalBudget = result.totalBudget, BallotDesign = result.ballot_type,
            Model = result.method
        };
        var election_created = await _electionService.CreateElectionAsync(election);
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
            var voter_model = new CreateVoter() { ElectionId = election_created.Id };
            var createdVoter = await _votersService.CreateVoterAsync(voter_model);
            //Adding votes
            for (int i = 0; i < pythonVoter.selectedProjects.Count; i++)
            {
                var project = pythonVoter.selectedProjects[i];
                var degree = pythonVoter.selectedDegree[i];
                //Adding Score to database
                var score = new Scores(){Grade = degree,  VoterId = createdVoter.Id, ProjectId = projectToIdMap[project]};
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
        

        return File(fileStream, "application/octet-stream", election.Name + "_custom.pb");
    }

    [HttpPost("analyze/CoherentGroups/{resultId}")]
    public async Task<Dictionary<Guid, Dictionary<string, float>>> GetGroupsAvgSat([FromBody] groupssatDto load,
        [FromRoute] Guid resultId)
    {
        var groups = load.groups;
        var sat = load.sats;
        var election = await _resultService.GetElectionResultByResultId(resultId);
        var pythonProjectElected = election.ElectedProjects.Select(p => new PythonProject()
        {
            name = p.Name, cost = p.Cost, categories = p.Categories.Select(c => c.Name).ToList() ?? [],
            target = p.Targets.Select(t => t.Name).ToList() ?? []
        }).ToList();
        var accSats = new Dictionary<Guid, Dictionary<string, float>>();
        var pythonVoters = new List<PythonVoter>();
        foreach (var group in groups)
        {
            //Create the group as 1 voter
            pythonVoters.Add(new PythonVoter()
                {
                    selectedProjects = group.projects.Select(p => p.Name).ToList(),
                    selectedDegree = group.projects.Select(_ => 1).ToList()
                }
            );
        }

        //Create the Election simulating only that group as voters
        var pythonElection = new PythonElection()
        {
            ballot_type = election.UsedBallot,
            method = election.UsedMethod,
            name = "NOT NEEDED",
            projects = election.SubmittedProjects.Select(p => new PythonProject()
                { cost = p.Cost, name = p.Name, categories = [], target = [] }).ToList(),
            totalBudget = election.TotalBudget,
            votes = pythonVoters
        };
        //Analyze avg satisfaction:
        var groupSat = await _service.GetAnalysisNumbersGroups(pythonElection, pythonProjectElected, sat);
        //Interpret result
        foreach (var gr in groupSat)
        {
            var noOfProjects = gr.Key.selectedProjects.Count();
            var chosenGuid = groups.Where(g => g.projects.Count() == noOfProjects).First(g =>
                g.projects.Select(p => p.Name).ToList().TrueForAll(name => gr.Key.selectedProjects.Contains(name))).id;
            ChangeKeysFromNumbersToReal(gr.Value);
            accSats.Add(chosenGuid, gr.Value);
        }

        return accSats;
    }


    [HttpPost("/analyze/avgSatisfaction/{resultId}")]
    public async Task<Dictionary<string, float>> GetAverageSatisfaction(Guid resultId, [FromBody] List<int> sats)
    {
        var election = await _resultService.GetElectionResultByResultId(resultId);
        var submittedPythonProjects = election.SubmittedProjects.Select(p =>
            new PythonProject()
            {
                name = p.Name, cost = p.Cost,
                categories = p.Categories?.Select(c => c.Name).ToList() ?? [],
                target = p.Targets?.Select(t => t.Name).ToList() ?? []
            }).ToList();


        var electedProjects = election.ElectedProjects.Select(p =>
            new PythonProject()
                {name =p.Name, cost = p.Cost, 
                    categories = p.Categories?.Select(c => c.Name).ToList() ?? [], target = p.Targets?.Select(t => t.Name).ToList() ??[]}).ToList();
       var voters = await _votersService.GetVotersByElectionId(election.ElectionId);
       var votersPython = voters.Select(v => new PythonVoter
       {
           selectedProjects = election.SubmittedProjects.Where(p => v.Votes.Select(r => r.ProjectId).ToList().ToList().Contains(p.Id)).ToList().ToList().Select(n => n.Name).ToList(),
           selectedDegree = v.Votes.Select(d => d.Grade).ToList()
       }).ToList();
        var pythonElection = new PythonElection()
        {
            ballot_type = election.UsedBallot,
            method = election.UsedBallot,
            name = "",
            projects = submittedPythonProjects,
            votes = votersPython,
            totalBudget = election.TotalBudget
        };
        
        var result = await _service.GetAnalysisNumbers(pythonElection, electedProjects, sats);
        ChangeKeysFromNumbersToReal(result);
        return result;

    }

    private void ChangeKeysFromNumbersToReal(Dictionary<string, float> dict)
    {
        var updates = new List<(string OldKey, string NewKey, float Value)>();

        foreach (var (key, value) in dict.Reverse())
        {
            if (Constants.sat_map.TryGetValue(key, out var newKey))
            {
                updates.Add((key, newKey, value));
            }
        }

// Apply updates after iteration
        foreach (var (oldKey, newKey, value) in updates)
        {
            dict[newKey] = value;
            dict.Remove(oldKey);
        }

    }
}