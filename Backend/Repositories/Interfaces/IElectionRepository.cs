using Backend.Models;
using DTO.Models;

namespace Backend.Repositories.Interfaces;

public interface IElectionRepository
{
    Task<IEnumerable<ElectionEntity>> GetAllAsync();
    
    Task<IEnumerable<ElectionEntity>> GetAllEndedAsync();
    
    Task<IEnumerable<ElectionEntity>> GetAllOpenAsync();
    Task EndElectionAsync(Guid id);
    Task<ElectionEntity?> GetByIdAsync(Guid id);

    Task<ElectionEntity> CreateAsync(CreateElectionModel election);

    Task<ElectionEntity?> UpdateAsync(ElectionEntity election);

    Task<bool> DeleteAsync(Guid id);
}