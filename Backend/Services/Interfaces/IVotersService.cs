using DTO.Models;

namespace Backend.Services.Interfaces;

public interface IVotersService
{
    Task<IEnumerable<Voter>> GetAllVotersAsync();

    Task<Voter?> GetVoterAsync(Guid id);
    
    Task<IEnumerable<Voter>> GetVotersByProjectIdAsync(int projectId);

    Task<Voter> CreateVoterAsync(CreateVoter voterModel);

    Task<Voter?> UpdateVoterAsync(Voter voterModel);

    Task<bool> DeleteByIdAsync(Guid id);
    Task<IEnumerable<Voter>> GetVotersByElectionId(Guid electionId);
}