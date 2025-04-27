using DTO.Models;
using Front.Components.ResultPage.CoherrentVoter;

namespace Front.Services.Interface;

public interface IVotersApiService
{
    Task<Voter> GetVoterById(Guid id);
    
    Task<List<Voter>> GetVotersByElectionId(Guid electionId);
    Task<int> CreateVoter(Guid electionId);
    
    Task<List<CoherrentVoter>> GetCoherentVotersByElectionId(Guid electionId, int noOfProjectsInGroup, int lowerbound);
}