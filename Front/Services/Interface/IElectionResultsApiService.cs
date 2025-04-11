using DTO.Models;

namespace Front.Services.Interface;

public interface IElectionResultsApiService
{
    /// <summary>
    /// Fetches the list of calculated results for an election
    /// </summary>
    /// <param name="electionId">The Guid of the <see cref="Election"/>
    ///     which the results is based upon </param>
    /// <returns>List containing the calculated results for an election</returns>
    Task<List<ElectionResult>?> GetResultsByElectionId(Guid electionId);
}