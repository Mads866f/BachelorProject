using DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services.Interfaces;

public interface IScoresService
{
    Task<IEnumerable<Scores>> GetScoresForVoterIdAsync(Guid id);
    
    Task<IEnumerable<Scores>> GetScoresForProjectIdAsync(Guid id);
    
    Task<Scores?> CreateVotersAsync(Scores voterModel);

    Task<Scores?> UpdateVotersAsync(Scores voterModel);

    Task<bool> DeleteByIdAsync(Guid voterId, Guid projectId);
}