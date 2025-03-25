using DTO.Models;

namespace Backend.Services.Interfaces;

public interface IVotersService
{
    Task<IEnumerable<Voter>> GetAllVotersAsync();

    Task<Voter?> GetVotersAsync(string id);

    Task<Voter> CreateVotersAsync(CreateVoter voterModel);

    Task<Voter?> UpdateVotersAsync(Voter voterModel);

    Task<bool> DeleteByIdAsync(string id);
}