using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Dapper;
using DTO.Models;

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
/// <summary>
///  Method for adding results of an Election to the database
/// </summary>
/// <param name="result">
///  The Election result to be created
/// </param>
/// <returns>
/// The ElectionResult as Created within the database
/// </returns>
    public async Task<ElectionResult> AddElectionResult(ElectionResult result)
    {
        _logger.LogInformation("Adding Election Result to database with Id: "+ result.ElectionId);
        
        //Add the result
        using var db = await dbFactory.CreateConnectionAsync();
        var query = """
                    INSERT INTO result_table (election_id, method_used, ballot_used)
                    VALUES (@ElectionId, @UsedMethod, @UsedBallot)
                    RETURNING id
                    """;
        var resultId = await db.QuerySingleAsync<Guid>(query,result);
        result.ElectionId = resultId;
       
        // Add the projects
        foreach (var project in result.ElectedProjects)
        {
            using var db2 =  await dbFactory.CreateConnectionAsync();
            const string query2 = """
                               INSERT INTO elected_projects_table(result_id, project_id) 
                               VALUES (@id, @project_id)
                               """;
            await db.QueryAsync(query2, new { id = result.ElectionId, project_id = project.Id });
        }
        return result;
    }
}