using AutoMapper;
using Backend.Models;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;
/// <summary>
/// Provides methods for managing elections, including CRUD operations.
/// Uses AutoMapper for DTO/entity conversions and delegates data access to the repository layer.
/// </summary>
public class ElectionService(IMapper mapper, IElectionRepository repository, ILogger<ElectionService> logger)
    : IElectionService
{
    private readonly ILogger<ElectionService> _logger = logger;

    /// <summary>
    /// Retrieves all elections.
    /// </summary>
    /// <returns>A collection of mapped election DTOs.</returns>
    public async Task<IEnumerable<Election>> GetAllElectionsAsync()
    {
    var result = await repository.GetAllAsync();
    return result
        .Select(mapper.Map<Election>); 
    }
    
    /// <summary>
    /// Retrieves a single election by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the election to retrieve.</param>
    /// <returns>The mapped election DTO if found; otherwise, null.</returns>
    public async Task<Election?> GetElectionAsync(Guid id)
    {
        var electionEntity = await repository.GetByIdAsync(id);
        if (electionEntity is null)
        {
            _logger.LogWarning("Election with ID {Id} not found", id);
            return null;
        }
        return mapper.Map<Election>(electionEntity);
    }

    /// <summary>
    /// Creates a new election.
    /// </summary>
    /// <param name="election">The model containing the election data to create.</param>
    /// <returns>The newly created election mapped as a DTO.</returns>
    public async Task<Election> CreateElectionAsync(CreateElectionModel election)
    {
        var electionEntity = await repository.CreateAsync(election);
        var electionDto = mapper.Map<Election>(electionEntity);
        return electionDto;
    }
    
    /// <summary>
    /// Updates an existing election.
    /// </summary>
    /// <param name="electionModel">The updated election data.</param>
    /// <returns>The updated election mapped as a DTO, or null if the update failed.</returns>
    public async Task<Election?> UpdateElectionAsync(Election electionModel)
    {
        var electionEntity = mapper.Map<ElectionEntity>(electionModel);
        var updated = await repository.UpdateAsync(electionEntity);
        if (updated is null)
        {
            _logger.LogWarning("Update failed: Election with ID {Id} not found", electionModel.Id);
            return null;
        }
        return mapper.Map<Election>(updated);
    }

    /// <summary>
    /// Deletes an election by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the election to delete.</param>
    /// <returns>True if deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var success = await repository.DeleteAsync(id);
        if (success is false)
        {
            _logger.LogWarning("Error deleting Election with Id {Id}", id);
            return false;
        }
        return true;
    }
}