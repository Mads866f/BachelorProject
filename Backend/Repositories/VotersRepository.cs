using System.Runtime.InteropServices;
using System.Text;
using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Utilities.DataStructures;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class VotersRepository(IDbConnectionFactory dbFactory, ILogger<VotersRepository> _logger) : IVotersRepository
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
        _logger.LogInformation("Getting List of Voters with Ids");
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
        _logger.LogInformation("Getting Voter with id:" + id);
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             SELECT id AS Id, election_id AS ElectionId
                             FROM voters_table
                             WHERE id = @idToFind LIMIT 1
                             """;
        var result = db.QuerySingleOrDefault<VoteEntity>(query, new { idToFind = id });
        if (result is not null)
        {
            _logger.LogInformation("Found a result with id:" + result.Id);
        }

        return result;
    }

    public async Task<VoteEntity> CreateAsync(CreateVoter voter)
    {
        _logger.LogInformation("Creating voter for election: " + voter.ElectionId);
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
        _logger.LogInformation("Updating voter: " + voter.Id);
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
        _logger.LogInformation("Deleting voter: " + id);
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             DELETE FROM voters_table
                             WHERE id = @Voter_Id
                             """;
        var rowsAffected = await db.ExecuteAsync(query, new { Id = id });
        if (rowsAffected != 0) return true;
            _logger.LogInformation($"Warning: Attempted to delete non-existing voter with Voter_Id {id}", id);
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
        VotesProjectPairsInElection(sql);
        var fromJoins = HandleProjectGroups(projectCount, out var selectFields, out var groupByFields);
        CountNumberOfVotersInGroup(sql, groupByFields, selectFields, fromJoins);
        FilterLowerBound(lowerBound, sql, groupByFields, selectFields, fromJoins);
        SelectProjectInformationInGroup(projectCount, sql);
        JoinTables(projectCount, sql);
        var resultingQuery = sql.ToString();
        Console.WriteLine("QUERY:\n"+resultingQuery+"\n");
        return resultingQuery;
    }

    private static void JoinTables(int projectCount, StringBuilder sql)
    {
        for (int i = 1; i <= projectCount; i++)
        {
            sql.AppendLine($"JOIN projects_table pr{i} ON pr{i}.id = cv.p{i}");
        }

        sql.AppendLine("ORDER BY " + string.Join(", ", Enumerable.Range(1, projectCount).Select(i => $"pr{i}.name")) +
                       ", cv.voter_id;");
    }

    private static void SelectProjectInformationInGroup(int projectCount, StringBuilder sql)
    {
        sql.AppendLine("SELECT");
        for (int i = 1; i <= projectCount; i++)
        {
            sql.AppendLine($"    pr{i}.name AS project{i}_name, pr{i}.cost AS project{i}_cost,");
        }
        sql.AppendLine("    cv.voter_id");
        sql.AppendLine("FROM coherent_voters cv");
    }

    private static void FilterLowerBound(int lowerBound, StringBuilder sql, List<string> groupByFields, List<string> selectFields,
        List<string> fromJoins)
    {
        sql.AppendLine("coherent_voters AS (");
        sql.AppendLine("    SELECT sub.voter_id, " + string.Join(", ", groupByFields.Select(p => "sub."+p)));
        sql.AppendLine("    FROM (");
        sql.AppendLine("        SELECT vp1.voter_id, " + string.Join(", ", selectFields));
        sql.AppendLine("        FROM " + string.Join("\n        ", fromJoins));
        sql.AppendLine("    ) sub");
        sql.AppendLine("    JOIN grouped g ON " + string.Join(" AND ", groupByFields.Select(p => $"sub.{p} = g.{p}")));
        sql.AppendLine($"    WHERE g.voter_count > {lowerBound}");
        sql.AppendLine(")");
    }

    private static void CountNumberOfVotersInGroup(StringBuilder sql, List<string> groupByFields, List<string> selectFields, List<string> fromJoins)
    {
        sql.AppendLine("grouped AS (");
        sql.AppendLine($"    SELECT {string.Join(", ", groupByFields)}, COUNT(DISTINCT voter_id) AS voter_count");
        sql.AppendLine("    FROM (");
        sql.AppendLine("        SELECT vp1.voter_id, " + string.Join(", ", selectFields));
        sql.AppendLine("        FROM " + string.Join("\n        ", fromJoins));
        sql.AppendLine("    ) sub");
        sql.AppendLine("    GROUP BY " + string.Join(", ", groupByFields));
        sql.AppendLine("),");
    }

    private static List<string> HandleProjectGroups(int projectCount, out List<string> selectFields, out List<string> groupByFields)
    {
        var fromJoins = new List<string>();
        selectFields = new List<string>();
        groupByFields = new List<string>();
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

        return fromJoins;
    }

    private static void VotesProjectPairsInElection(StringBuilder sql)
    {
        sql.AppendLine(@"
WITH voter_projects AS (
    SELECT s.voter_id, s.project_id
    FROM scores_table s
    JOIN voters_table v ON s.voter_id = v.id
    WHERE v.election_id = @ElectionId
),");
    }


    public async Task<Dictionary<List<Project>,List<Guid>>> GetKSizeCoherentVotersFromElection(Guid electionId, int noOfProjectsInGroup, int lowerBound)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        // 1) Build the SQL with a parameter placeholder @ElectionId
        var sql = BuildCoherentVoterQuery(noOfProjectsInGroup, lowerBound);

        // 2) Dapper will return a dynamic row with columns:
        //    project1_name, project1_cost, ..., projectN_name, projectN_cost, voter_count
        var rows = await db.QueryAsync<dynamic>(sql, new { ElectionId = electionId });
        
        var result =  new Dictionary<string, List<Guid>>();
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
            projectList.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            var voterId = (Guid)((IDictionary<string,object>) row)["voter_id"];
            var projectKey = projectList.Select(p => $"{p.Name}:{p.Cost}").Aggregate((i, j) => $"{i}|{j}");
            if (!result.TryGetValue(projectKey, out var voterList))
            {
                voterList = new List<Guid>();
                result[projectKey] = voterList;
            }
            voterList.Add(voterId);
        }
        var resultDict = new Dictionary<List<Project>, List<Guid>>();
        foreach (var kvp in result)
        {
            var key = kvp.Key;
            var keyTransformed = key.Split('|').Select(p => p.Replace("|","")).ToList();
            var projectKey = keyTransformed.Select(p=> p.Split(":").Select(k => k.Replace(":",""))
            
            ).Select(q => 
                    new Project(){Categories = [], Cost = int.Parse(q.Last()), Name =q.First() , ElectionId = electionId, Id = Guid.Empty, Targets = [], votes = 0}
                ).ToList(); 
            resultDict.Add(projectKey,kvp.Value);
        };
        return resultDict;
    }


    public Task<IEnumerable<VoteEntity>> GetVotersByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }
}