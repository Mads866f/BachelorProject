using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Dapper;

namespace Backend.Repositories;

public class ElectionResultRepository(IDbConnectionFactory dbFactory, ILogger<ElectionResultRepository> _logger, IProjectsRepository _projectsRepository): IElectionResultRepository
{
    /// <summary>
    ///  Requests all Election Results with the given electionId from the database
    /// </summary>
    /// <param name="electionId">
    /// The Id of the election which results are wanted
    /// </param>
    /// <returns>
    /// A list of associated Election Results.
    /// </returns>
    public async Task<IEnumerable<ElectionResultEntity>> GetElectionsResultByElectionId(Guid electionId)
    {
        _logger.LogInformation("Getting ElectionResults from database with Id: "+ electionId);
        using var db = await dbFactory.CreateConnectionAsync();
        var query = """
                    SELECT id as Id, election_id as ElectionId, method_used as MethodUsed, ballot_used as BallotUsed
                    FROM result_table as result
                    WHERE result.election_id = @electionId
                    """;
        var result = await db.QueryAsync<ElectionResultEntity>(query, new { electionId = electionId });
        return result;
    }
/// <summary>
/// requests a list of projects that have been elected for the given result from the database.
/// </summary>
/// <param name="resultId">
/// The Id of the result for which the elected projects is wanted.
/// </param>
/// <returns>
/// List of elected Projects
/// </returns>
    public async Task<IEnumerable<ProjectsEntity>> GetProjectsByResultId(Guid resultId)
    {
        _logger.LogInformation("Getting Elected Projects from database with Id: "+ resultId);
        using var db = await dbFactory.CreateConnectionAsync();
        //Query for getting project Id
        var query = """
                    SELECT project_id
                    FROM elected_projects_table as ep
                    Where ep.result_id = @resultId
                    """;
        var projectIds = await db.QueryAsync<Guid>(query, new { resultId = resultId });
       
        //Query for getting Projects
        var projects = new List<ProjectsEntity>();
        foreach (var projectId in projectIds)
        {
            var p = await _projectsRepository.GetByIdAsync(projectId);
            if (p is null){continue;}
            projects.Add(p);
        }
        return projects;
    }
}