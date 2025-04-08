using Backend.Models;
using DTO.Models;

namespace Backend.Repositories.Interfaces;

public interface IElectionRepository
{
    Task<IEnumerable<ElectionEntity>> GetAllAsync();

    Task<ElectionEntity?> GetByIdAsync(Guid id);

    Task<ElectionEntity> CreateAsync(Election election);

    Task<ElectionEntity?> UpdateAsync(ElectionEntity election);

    Task<bool> DeleteAsync(Guid id);
}