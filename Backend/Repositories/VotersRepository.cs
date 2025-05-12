using System.Runtime.InteropServices;
using System.Text;
using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Utilities.DataStructures;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class VotersRepository(IDbConnectionFactory dbFactory) : IVotersRepository
{
    public async Task<IEnumerable<VoteEntity>> GetAllAsync()
    {
        using var db = await dbFactory.CreateConnectionAsync();

        const string query = """
                                 SELECT 
                                     v.id AS Id, v.election_id AS ElectionId,

                                     s.voter_id AS Voter_Id, s.project_id AS Project_Id, s.grade AS Grade,

                                     p.id AS Id, p.election_id AS ElectionId, p.name AS Name, p.cost AS Cost

                                 FROM voters_table v
                                 LEFT JOIN scores_table s ON s.voter_id = v.id
                                 LEFT JOIN projects_table p ON p.id = s.project_id;
                             """;

        var voterDict = new Dictionary<Guid, VoteEntity>();

        await db.QueryAsync<VoteEntity, ScoresEntity, ProjectsEntity, VoteEntity>(
            query,
            (voter, score, project) =>
            {
                if (!voterDict.TryGetValue(voter.Id, out var existingVoter))
                {
                    existingVoter = voter;
                    existingVoter.ScoresEntities = new List<ScoresEntity>();
                    voterDict.Add(existingVoter.Id, existingVoter);
                }

                if (score != null && score.Voter_Id != Guid.Empty)
                {
                    // Only assign the project if it is present (optional but safer)
                    if (project != null && project.Id != Guid.Empty)
                        score.ProjectsEntity = project;

                    existingVoter.ScoresEntities.Add(score);
                }

                return existingVoter;
            },
            splitOn: "Voter_Id,Id"
        );

        return voterDict.Values;
    }

    public async Task<IEnumerable<VoteEntity>> GetVotersWithIdListAsync(List<Guid> votersIdList)
    {
        Console.WriteLine("Getting List of Voters with Ids");
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                                 SELECT 
                                     v.id AS Id, v.election_id AS ElectionId,

                                     s.voter_id AS Voter_Id, s.project_id AS Project_Id, s.grade AS Grade,

                                     p.id AS Id, p.election_id AS ElectionId, p.name AS Name, p.cost AS Cost

                                 FROM voters_table v
                                 LEFT JOIN scores_table s ON s.voter_id = v.id
                                 LEFT JOIN projects_table p ON p.id = s.project_id
                                 WHERE v.id = ANY(@Ids);
                             """;
        var voterDict = new Dictionary<Guid, VoteEntity>();
        await db.QueryAsync<VoteEntity, ScoresEntity, ProjectsEntity, VoteEntity>(
            query,
            (voter, score, project) =>
            {
                if (!voterDict.TryGetValue(voter.Id, out var existingVoter))
                {
                    existingVoter = voter;
                    existingVoter.ScoresEntities = new List<ScoresEntity>();
                    voterDict.Add(existingVoter.Id, existingVoter);
                }

                if (score != null && score.Voter_Id != Guid.Empty)
                {
                    // Only assign the project if it is present (optional but safer)
                    if (project != null && project.Id != Guid.Empty)
                        score.ProjectsEntity = project;

                    existingVoter.ScoresEntities.Add(score);
                }

                return existingVoter;
            },
            new {Ids = votersIdList},
            splitOn: "Voter_Id,Id"
        );
        return voterDict.Values; 
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
        var result = db.QuerySingleOrDefault<VoteEntity>(query, new { idToFind = id });
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
        var rowsAffected = await db.ExecuteAsync(query, new { Id = id });
        if (rowsAffected != 0) return true;
        Console.WriteLine($"Warning: Attempted to delete non-existing voter with Voter_Id {id}", id);
        return false;
    }

    public async Task<IEnumerable<VoteEntity>> GetByElectionIdAsync(Guid electionId)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        const string query = """
                                 SELECT 
                                     v.id AS Id, v.election_id AS ElectionId,

                                     s.voter_id AS Voter_Id, s.project_id AS Project_Id, s.grade AS Grade,

                                     p.id AS Id, p.election_id AS ElectionId, p.name AS Name, p.cost AS Cost

                                 FROM voters_table v
                                 LEFT JOIN scores_table s ON s.voter_id = v.id
                                 LEFT JOIN projects_table p ON p.id = s.project_id
                                 WHERE v.election_id = @ElectionId;
                             """;

        var voterDict = new Dictionary<Guid, VoteEntity>();

        await db.QueryAsync<VoteEntity, ScoresEntity, ProjectsEntity, VoteEntity>(
            query,
            (voter, score, project) =>
            {
                if (!voterDict.TryGetValue(voter.Id, out var existingVoter))
                {
                    existingVoter = voter;
                    existingVoter.ScoresEntities = new List<ScoresEntity>();
                    voterDict.Add(existingVoter.Id, existingVoter);
                }

                if (score != null && score.Voter_Id != Guid.Empty)
                {
                    if (project != null && project.Id != Guid.Empty)
                        score.ProjectsEntity = project;

                    existingVoter.ScoresEntities.Add(score);
                }

                return existingVoter;
            },
            new { ElectionId = electionId },
            splitOn: "Voter_Id,Id"
        );

        return voterDict.Values;
    }

    private static string BuildCoherentVoterQuery(int projectCount, int lowerBound)
    {
        if (projectCount < 2)
            throw new ArgumentException("Project count must be at least 2.");

        var sql = new StringBuilder();

        // 1) Base CTE: voter-project assignments for this election
        sql.AppendLine(@"
WITH voter_projects AS (
    SELECT s.voter_id, s.project_id
    FROM scores_table s
    JOIN voters_table v ON s.voter_id = v.id
    WHERE v.election_id = @ElectionId
),");

        // 2) Build inner grouping CTE ("grouped")
        var fromJoins = new List<string>();
        var selectFields = new List<string>();
        var groupByFields = new List<string>();
        var joinProjects = new List<string>();

        for (int i = 1; i <= projectCount; i++)
        {
            selectFields.Add($"vp{i}.project_id AS p{i}");

            if (i == 1)
                fromJoins.Add("voter_projects vp1");
            else
                fromJoins.Add($@"JOIN voter_projects vp{i}
        ON vp1.voter_id = vp{i}.voter_id
        AND vp{i - 1}.project_id < vp{i}.project_id");

            groupByFields.Add($"p{i}");
            joinProjects.Add($@"JOIN projects_table pr{i}
    ON pr{i}.id = p{i}");
        }

        // 2e) grouped CTE
        sql.AppendLine("grouped AS (");
        sql.AppendLine($"    SELECT {string.Join(", ", groupByFields)}, COUNT(DISTINCT voter_id) AS voter_count");
        sql.AppendLine("    FROM (");
        sql.AppendLine("        SELECT vp1.voter_id, " + string.Join(", ", selectFields));
        sql.AppendLine("        FROM " + string.Join("\n        ", fromJoins));
        sql.AppendLine("    ) sub");
        sql.AppendLine("    GROUP BY " + string.Join(", ", groupByFields));
        sql.AppendLine("),");

        // 3) coherent_voters CTE
        sql.AppendLine("coherent_voters AS (");
        sql.AppendLine("    SELECT sub.voter_id, " + string.Join(", ", groupByFields.Select(p => "sub."+p)));
        sql.AppendLine("    FROM (");
        sql.AppendLine("        SELECT vp1.voter_id, " + string.Join(", ", selectFields));
        sql.AppendLine("        FROM " + string.Join("\n        ", fromJoins));
        sql.AppendLine("    ) sub");
        sql.AppendLine("    JOIN grouped g ON " + string.Join(" AND ", groupByFields.Select(p => $"sub.{p} = g.{p}")));
        sql.AppendLine($"    WHERE g.voter_count > {lowerBound}");
        sql.AppendLine(")");

        // 4) Final SELECT
        sql.AppendLine("SELECT");
        for (int i = 1; i <= projectCount; i++)
        {
            sql.AppendLine($"    pr{i}.name AS project{i}_name, pr{i}.cost AS project{i}_cost,");
        }

        sql.AppendLine("    cv.voter_id");
        sql.AppendLine("FROM coherent_voters cv");

        // 5) Join to projects_table
        for (int i = 1; i <= projectCount; i++)
        {
            sql.AppendLine($"JOIN projects_table pr{i} ON pr{i}.id = cv.p{i}");
        }

        sql.AppendLine("ORDER BY " + string.Join(", ", Enumerable.Range(1, projectCount).Select(i => $"pr{i}.name")) +
                       ", cv.voter_id;");

        Console.WriteLine("SQL QUERY BUILD:\n" +  sql.ToString());
        Console.WriteLine();
        return sql.ToString();
    }


    public async Task<Dictionary<List<Project>,List<Guid>>> GetKSizeCoherentVotersFromElection(Guid electionId, int noOfProjectsInGroup, int lowerBound)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        // 1) Build the SQL with a parameter placeholder @ElectionId
        var sql = BuildCoherentVoterQuery(noOfProjectsInGroup, lowerBound);

        // 2) Dapper will return a dynamic row with columns:
        //    project1_name, project1_cost, ..., projectN_name, projectN_cost, voter_count
        var rows = await db.QueryAsync<dynamic>(sql, new { ElectionId = electionId });
        
        var result =  new Dictionary<List<Project>, List<Guid>>(new ListKey<Project>());
        foreach (var row in rows)
        {
            var projectList = new  List<Project>();
            for (int i = 1; i <= noOfProjectsInGroup; i++)
            {
                var nameProp = $"project{i}_name";
                var costProp = $"project{i}_cost";

                var name = (string)((IDictionary<string, object>)row)[nameProp];
                var cost = Convert.ToInt32(((IDictionary<string, object>)row)[costProp]);
                projectList.Add(
                    new Project(){Categories = [], Cost = cost, Name = name, ElectionId = electionId, Id = Guid.Empty, Targets = [], votes = 0}
                    );
            }
            var voterId = (Guid)((IDictionary<string,object>) row)["voter_id"];
            var projectKey = projectList.OrderBy(p => p.Name).ToList();
            if (!result.TryGetValue(projectKey, out var voterList))
            {
                voterList = new List<Guid>();
                result[projectKey] = voterList;
            }
            voterList.Add(voterId);
        }
        return result;
    }


    public Task<IEnumerable<VoteEntity>> GetVotersByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }
}