using System.Data;
using Backend.Database;
using Backend.Models;
using Dapper;
using DTO.Models;

namespace Backend.Repositories.Interfaces;

public class ScoresRepository(IDbConnectionFactory dbFactory, ILogger<ScoresRepository> _logger) : IScoresRepository
{
    public async Task<IEnumerable<ScoresEntity>> GetScoreForVoter(Guid voterId)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        var result =  await db.QueryAsync<ScoresEntity>(
            """
            SELECT voter_id AS Voter_Id, project_id AS Project_Id, grade AS Grade
            FROM scores_table
            WHERE voter_id = @idToLookUp
            """, new { idToLookUp = voterId }
            );

        return result;
    }

    public async Task<IEnumerable<ScoresEntity>> GetScoreForProject(Guid projectId)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        return await db.QueryAsync<ScoresEntity>(
            """
            SELECT voter_id AS VoterId, project_id AS Project_Id, grade AS Grade
            FROM scores_table
            WHERE project_id = @idToLookUp
            """, new { idToLookUp = projectId }
            );
    }

    public async Task<ScoresEntity?> GetByIdAsync(Guid voterId, Guid projectId)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        const string query =
            """
            SELECT voter_id AS VoterId, project_id AS projectID, grade AS Grade
            FROM scores_table
            WHERE voter_id = @VoterIdLookUp AND project_id = @ProjectIdLookUp LIMIT 1
            """;

        return await db.QuerySingleOrDefaultAsync<ScoresEntity>(query,
            new { VoterIdLookUp = voterId, ProjectIdLookUp = projectId });
    }

    public async Task<ScoresEntity> CreateAsync(Scores scores)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        const string query =
            """
            INSERT INTO scores_table (voter_id, project_id, grade)
            VALUES (@Voter_Id, @Project_Id, @Grade)
            RETURNING voter_id AS Voter_Id , project_id AS Project_Id;
            """;
        var result = await db.QuerySingleAsync<ScoresEntity>(query, scores);

        return new ScoresEntity()
        {
            Voter_Id = result.Voter_Id,
            Project_Id = result.Project_Id,
            Grade = scores.Grade
        };
    }

    public async Task<ScoresEntity?> UpdateAsync(ScoresEntity voter)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        const string query =
            """
            UPDATE scores_table
            SET grade = @Grade
            WHERE voter_id = @VoterId AND project_id = @Project_Id
            """;
        await db.ExecuteAsync(query, new {VoterId = voter.Voter_Id, ProjectId = voter.Project_Id});
        return await GetByIdAsync(voter.Voter_Id, voter.Project_Id);
    }

    public async Task<bool> DeleteAsync(Guid voterId, Guid projectId)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        var rowsAffected = await db.ExecuteAsync(
            """
            DELETE
            FROM scores_table
            WHERE voter_id = @VoterId AND project_id = @Project_Id
            """, new {VoterId = voterId, Project_Id = projectId});
        if (rowsAffected != 0) return true;
        _logger.LogInformation($"Warning: Attempted to delete non-existing score with voterId: {voterId} and project id: {projectId}", voterId, projectId);
        return false;
    }
}