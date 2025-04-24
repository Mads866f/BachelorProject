using Backend.Models;
using DTO.Models;

namespace Backend.Repositories.Interfaces;

public interface IScoresRepository
{
    Task<IEnumerable<ScoresEntity>> GetScoreForVoter(Guid parse);
    
    Task BatchGetScoreForVoter(List<Guid> voterIds);
    Task<IEnumerable<ScoresEntity>> GetScoreForProject(Guid parse);

    Task<ScoresEntity?> GetByIdAsync(Guid voterId, Guid projectId);

    Task<ScoresEntity> CreateAsync(Scores election);

    Task<ScoresEntity?> UpdateAsync(ScoresEntity voter);

    Task<bool> DeleteAsync(Guid voterId, Guid projectId);
}