using DTO.Models;

namespace Front.Services.Interface;

public interface IVotersApiService
{
    Task<Voter> GetVoter(string id);
    
    Task<List<Voter>> GetVoters(string electionId);
}