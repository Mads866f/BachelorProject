using DTO.Models;

namespace Front.Services.Interface;

public interface IPbEngineApiService
{
    Task<List<Project>> CalculateElection(string electionId);
}