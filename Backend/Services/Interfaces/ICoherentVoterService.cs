using DTO.Models;

namespace Backend.Services.Interfaces;

/// <summary>
/// Provides functionality for calculating coherent voter groups based on election data.
/// </summary>
public interface ICoherentVoterService
{
    /// <summary>
    /// Calculates the coherent voter groups based on the specified election ID.
    /// </summary>
    /// <param name="electionId">The unique identifier of the election for which coherent voter groups need to be calculated.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of coherent voter groups.</returns>
    Task<List<CoherentVoterGroup>> CalculateCoherentVotersAsync(Guid electionId);
}