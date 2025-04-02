using DTO.Models;

namespace Front.Services.Interface;

public interface IScoresApiService
{
    Task UpdateScores(string voterId, Dictionary<string, int> votes);
}