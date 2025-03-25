using Backend.Models;
using DTO.Models;

namespace Backend.Repositories.Interfaces;

public interface IVotersRepository
{
    Task<IEnumerable<VoteEntity>> GetAllAsync();

    Task<VoteEntity?> GetByIdAsync(Guid id);

    Task<VoteEntity> CreateAsync(CreateVoter election);

    Task<VoteEntity?> UpdateAsync(VoteEntity voter);

    Task<bool> DeleteAsync(Guid id);
}