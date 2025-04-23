using DTO.Models;

namespace Backend.Services.Interfaces;

/// <summary>
/// Provides operations related to election results, including retrieval and addition of results.
/// </summary>
public interface IElectionResultService
{
    /// <summary>
    /// Retrieves a list of election results for the specified election ID.
    /// </summary>
    /// <param name="electionId">The unique identifier of the election for which results are to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation.
    /// The task result contains a list of <see cref="ElectionResult"/> corresponding to the specified election ID.</returns>
    Task<List<ElectionResult>> GetElectionsResultsByElectionId(Guid electionId);

    /// <summary>
    /// Adds a new election result to the repository.
    /// </summary>
    /// <param name="result">The election result object containing details such as election ID,
    /// submitted projects, elected projects, used method, and ballot type.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the added <see cref="ElectionResult"/> object.</returns>
    Task<ElectionResult> AddElectionResult(ElectionResult result);
}