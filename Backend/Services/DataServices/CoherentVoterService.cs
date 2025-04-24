using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;

/// <inheritdoc cref="ICoherentVoterService"/>
public class CoherentVoterService(IElectionService electionService,
    IProjectService projectService,
    IVotersService votersService,
    IScoresService scoresService) 
    : ICoherentVoterService
{
    /// <inheritdoc/>
    public async Task<List<CoherentVoterGroup>> CalculateCoherentVotersAsync(Guid electionId)
    {
        // TODO Create ResultType which can contain both data and error
        var election = await electionService.GetElectionAsync(electionId);
        // Bailout if election does not exist
        if (election == null) return [];
        
        // Fetch List of all Voters
        var voters = await votersService.GetVotersByElectionId(electionId);
        
        
        
        
        throw new NotImplementedException();
    }
}