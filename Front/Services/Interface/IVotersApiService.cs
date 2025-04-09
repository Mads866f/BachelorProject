using DTO.Models;

namespace Front.Services.Interface;

public interface IVotersApiService
{
    Task<Voter> GetVoterById(Guid id);
    
    Task<List<Voter>> GetVotersByElectionId(Guid electionId);
    Task<int> CreateVoter(Guid electionId);
}