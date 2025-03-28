using DTO.Models;

namespace Backend.Services.Interfaces;

public interface IScoresService
{
    Task<IEnumerable<Scores>> GetScoresForVoterIdAsync(string id);
    
    Task<IEnumerable<Scores>> GetScoresForProjectIdAsync(string id);
    
    Task<Scores> CreateVotersAsync(Scores voterModel);

    Task<Scores?> UpdateVotersAsync(Scores voterModel);

    Task<bool> DeleteByIdAsync(string voterId, string projectId);
}