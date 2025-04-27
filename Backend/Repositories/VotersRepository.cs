using System.Runtime.InteropServices;
using System.Text;
using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class VotersRepository(IDbConnectionFactory dbFactory, GlobalDatabaseSemaphore _semaphore) : IVotersRepository
{
    private IVotersRepository _votersRepositoryImplementation;

    public async Task<IEnumerable<VoteEntity>> GetAllAsync()
    {
        await _semaphore.semaphore.WaitAsync();
        IEnumerable<VoteEntity> result;
        try
        {
            using var db = await dbFactory.CreateConnectionAsync();
            const string query = """
                                 SELECT id AS Id, election_id AS ElectionId
                                 FROM voters_table
                                 """;
            result = await db.QueryAsync<VoteEntity>(query);
        }
        finally
        {
            _semaphore.semaphore.Release();
        }

        return result;
    }

    public async Task<VoteEntity?> GetByIdAsync(Guid id)
    {
        Console.WriteLine("Getting Voter with id:" + id);
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             SELECT id AS Id, election_id AS ElectionId
                             FROM voters_table
                             WHERE id = @idToFind LIMIT 1
                             """;
        var result =db.QuerySingleOrDefault<VoteEntity>(query, new {idToFind = id});
        if (result is not null)
        {
            Console.WriteLine("Found a result with id:" + result.Id);
        }
        return result;
    }

    public async Task<VoteEntity> CreateAsync(CreateVoter voter)
    {
        Console.WriteLine("Creating voter for election: " + voter.ElectionId);
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             INSERT INTO voters_table (election_id)
                             VALUES (@ElectionId)
                             RETURNING id;
                             """;
        var id = await db.QuerySingleAsync<Guid>(query, voter);

        return new VoteEntity
        {
            Id = id,
            ElectionId = voter.ElectionId
        };
    }

    public async Task<VoteEntity?> UpdateAsync(VoteEntity voter)
    {
        Console.WriteLine("Updating voter: " + voter.Id);
        using var db = await dbFactory.CreateConnectionAsync();
    
        const string query = """
                             UPDATE voters_table
                             SET election_id = @ElectionId
                             WHERE id = @Voter_Id
                             """;
        await db.ExecuteAsync(query, voter);
        return await GetByIdAsync(voter.Id) ?? null;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        Console.WriteLine("Deleting voter: " + id);
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             DELETE FROM voters_table
                             WHERE id = @Voter_Id
                             """;
        var rowsAffected = await db.ExecuteAsync(query, new {Id = id});
        if (rowsAffected != 0) return true;
        Console.WriteLine($"Warning: Attempted to delete non-existing voter with Voter_Id {id}",id);
        return false;
    }

    public async Task<IEnumerable<VoteEntity>> GetByElectionIdAsync(Guid electionId)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             SELECT id as Id, election_id as ElectionId 
                             FROM voters_table
                             Where election_id = @ElectionId
                             """;
        var result = await db.QueryAsync<VoteEntity>(query, new {ElectionId = electionId});
        return result;
        
    }

private static string BuildCoherentVoterQuery(int projectCount,int lowerBound)
{
    if (projectCount < 2)
        throw new ArgumentException("Project count must be at least 2.");

    var sql = new StringBuilder();

    // 1) Base CTE
    sql.AppendLine(@"
WITH voter_projects AS (
    SELECT s.voter_id, s.project_id
    FROM scores_table s
    JOIN voters_table v ON s.voter_id = v.id
    WHERE v.election_id = @ElectionId
),");

    // 2) Build the inner grouping CTE ("grouped")
    //    We need the FROM/JOIN chain, the SELECT fields, and the GROUP BY keys.
    var fromJoins     = new List<string>();
    var selectFields  = new List<string>();
    var groupByFields = new List<string>();
    var joinProjects  = new List<string>();

    for (int i = 1; i <= projectCount; i++)
    {
        // 2a) projection in the subquery
        selectFields.Add($"vp{i}.project_id AS p{i}");

        // 2b) build the FROM / JOIN chain
        if (i == 1)
            fromJoins.Add("voter_projects vp1");
        else
            fromJoins.Add($@"JOIN voter_projects vp{i}
    ON vp1.voter_id = vp{i}.voter_id
   AND vp{i - 1}.project_id < vp{i}.project_id");

        // 2c) what we group by at the end
        groupByFields.Add($"p{i}");

        // 2d) how we join back to get names & costs
        joinProjects.Add($@"JOIN projects_table pr{i}
    ON pr{i}.id = p{i}");
    }

    // 2e) emit the grouped CTE
    sql.AppendLine("grouped AS (");
    sql.AppendLine($"    SELECT {string.Join(", ", groupByFields)}, COUNT(DISTINCT voter_id) AS voter_count");
    sql.AppendLine("    FROM (");
    sql.AppendLine("        SELECT vp1.voter_id, " + string.Join(", ", selectFields));
    sql.AppendLine("        FROM " + string.Join("\n        ", fromJoins));
    sql.AppendLine("    ) sub");
    sql.AppendLine("    GROUP BY " + string.Join(", ", groupByFields));
    sql.AppendLine(")");

    // 3) Final SELECT: projectN_name, projectN_cost, voter_count
    sql.AppendLine("SELECT");
    for (int i = 1; i <= projectCount; i++)
    {
        sql.Append($"    pr{i}.name AS project{i}_name, pr{i}.cost AS project{i}_cost");
        sql.AppendLine(i < projectCount ? "," : "");
    }
    sql.AppendLine(",   voter_count");
    sql.AppendLine("FROM grouped");
    
    // 4) Append all project joins
    foreach (var join in joinProjects)
        sql.AppendLine(join);

    sql.AppendLine($"WHERE voter_count > {lowerBound}");
    sql.AppendLine("ORDER BY voter_count DESC;");

    return sql.ToString();
}

public async Task<IEnumerable<(IEnumerable<ProjectsEntity> Projects, int VoterCount)>>
  GetKSizeCoherentVotersFromElection(Guid electionId, int projectCount, int lowerBound)
{
    using var db = await dbFactory.CreateConnectionAsync();

    // 1) Build the SQL with a parameter placeholder @ElectionId
    var sql = BuildCoherentVoterQuery(projectCount,lowerBound);

    // 2) Dapper will return a dynamic row with columns:
    //    project1_name, project1_cost, ..., projectN_name, projectN_cost, voter_count
    var rows = await db.QueryAsync<dynamic>(sql, new { ElectionId = electionId });

    var result = new List<(IEnumerable<ProjectsEntity>, int)>();

    foreach (var row in rows)
    {
        // Build the list of N ProjectsEntity
        var dict = (IDictionary<string, object>)row;
        var projects = new List<ProjectsEntity>(projectCount);
        for (int i = 1; i <= projectCount; i++)
        {
            projects.Add(new ProjectsEntity
            {
                Name       = (string)dict[$"project{i}_name"],
                Cost       = (int)dict[$"project{i}_cost"],
                votes      = 0,
                ElectionId = electionId,
                Id         = Guid.NewGuid()
            });
        }

        // Read the voter_count
        int count = (int)row.voter_count;
        result.Add((projects, count));
    }

    return result;
}
    
    
    
    public Task<IEnumerable<VoteEntity>> GetVotersByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }

}