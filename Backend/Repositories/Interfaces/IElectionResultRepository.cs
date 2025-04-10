using Backend.Models;

namespace Backend.Repositories.Interfaces;

public interface IElectionResultRepository
{
    Task<IEnumerable<ElectionResultEntity>> GetElectionsResultByElectionId(Guid electionId);
   
    Task<IEnumerable<ProjectsEntity>> GetProjectsByResultId(Guid resultId);
}