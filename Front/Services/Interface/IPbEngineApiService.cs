using DTO.Models;

namespace Front.Services.Interface;

public interface IPbEngineApiService
{
    Task<List<Project>> CalculateElection(Guid electionId);
    Task<List<string>> GetRealElectionsNames();

    Task<Election> DownloadRealElection(string nameOfElection);
    
    Task<string> DownloadCustomElection(Election electionId);
}