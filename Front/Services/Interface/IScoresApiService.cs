using DTO.Models;
using MudBlazor;

namespace Front.Services.Interface;

public interface IScoresApiService
{
    Task<int> UpdateScores(Guid voterId, Dictionary<string, int> votes);
}