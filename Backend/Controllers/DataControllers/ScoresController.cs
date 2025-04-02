using System.Runtime.InteropServices;
using Backend.Models;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/scores")]
public class ScoresController(IScoresService service) : ControllerBase
{
    private readonly IScoresService _service = service;
    
    [HttpPost("{voterid}")]
    public async Task UpdateScores(string voterid, [FromBody] Dictionary<string, int> scores)
    {
        Console.WriteLine("Updating Scores (Backend(Controller))");
        Console.WriteLine("VoterId: "+voterid);
        //Delete existing votes
        var projects_id = scores.Keys.ToList();
        var voterIdGuid = Guid.Parse(voterid);
        
        //Previous Votes
        var prevVotes = await _service.GetScoresForVoterIdAsync(voterid);
        Console.WriteLine("OLD PROJ:"+ prevVotes.Count());
        
        foreach (var score in prevVotes)
        {
            Console.WriteLine("SCORE: " + score.Voter_Id + " : " + score.Project_Id);
            await _service.DeleteByIdAsync(voterid,score.Project_Id.ToString());
        }
        //Add new votes
        foreach (var project in projects_id)
        {
            Console.WriteLine("ID: "+project);
            var score = new Scores(){Grade = scores[project],Voter_Id = voterIdGuid,Project_Id = Guid.ParseExact(project,"D")};
            await _service.CreateVotersAsync(score);
        }
        
       Console.WriteLine("Updating Scores (Backend(Controller))"); 
    }
}