using Backend.Models;
using DTO.Models;

namespace Backend.Repositories.Interfaces;

public interface IElectionResultRepository
{
    Task<IEnumerable<ElectionResultEntity>> GetElectionsResultByElectionId(Guid electionId);
   
    Task<IEnumerable<ProjectsEntity>> GetProjectsByResultId(Guid resultId);
    
    Task<ElectionResult> AddElectionResult(ElectionResult result);
    Task<ElectionResultEntity> GetElectionResultByResultId(Guid resultId);
}