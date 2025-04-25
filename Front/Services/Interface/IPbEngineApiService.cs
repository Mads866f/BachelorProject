using DTO.Models;
using Front.Components.ResultPage.CoherrentVoter;

namespace Front.Services.Interface;

public interface IPbEngineApiService
{
    Task<List<Project>> CalculateElection(Guid electionId);
    Task<List<string>> GetRealElectionsNames();

    Task<Election> DownloadRealElection(string nameOfElection);
    
    Task<string> DownloadCustomElection(Election electionId);

    Task<Dictionary<string, float>> GetAvgSatisfactions(ElectionResult electionResult);


    Task<Dictionary<Guid, Dictionary<string, float>>> GetAvgSatisfactionCoherentGroups(List<CoherrentVoter> coherrents,
        ElectionResult electionResult);
}