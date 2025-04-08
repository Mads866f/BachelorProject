using DTO.Models;

namespace Backend.Services.Interfaces;

public interface IElectionService
{
    Task<IEnumerable<Election>> GetAllElectionsAsync();

    Task<Election?> GetElectionAsync(Guid id);

    Task<Election> CreateElectionAsync(CreateElectionModel election);

    Task<Election?> UpdateElectionAsync(Election electionModel);

    Task<bool> DeleteByIdAsync(Guid id);
}