using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class ProjectRepository(IDbConnectionFactory dbFactory ,ILogger<ProjectRepository> _logger) : IProjectsRepository
{
   public async Task<IEnumerable<ProjectsEntity>> GetByElectionID(Guid electionID)
   {
      _logger.LogInformation("Getting projects from database for election with id: " + electionID);
      using var db = await dbFactory.CreateConnectionAsync();

      var query = await db.QueryAsync<ProjectsEntity>(
          """
          SELECT 
              p.id AS Id, 
              p.name AS Name, 
              p.election_id AS ElectionId, 
              p.cost AS Cost,
              COUNT(s.project_id) AS Votes
          FROM projects_table AS p
          LEFT JOIN scores_table AS s ON p.id = s.project_id
          WHERE p.election_id = @idAsGuid
          GROUP BY p.id, p.name, p.election_id, p.cost
          ORDER BY Votes DESC
          """,
          new { idAsGuid = electionID });
         
      
      return query;
   }

   public async Task<Project> CreateAsync(ProjectsEntity project)
   {
      using var db = await dbFactory.CreateConnectionAsync();
      const string query = """
                           INSERT INTO projects_table (election_id, name, cost)
                           Values (@ElectionId, @Name, @Cost)
                           RETURNING id
                           """;
      var projectId = await db.QuerySingleAsync<Guid>(query, project);
      var toReturn =  new Project
      {
         Id = projectId,
         ElectionId = project.ElectionId,
         Name = project.Name,
         Cost = project.Cost,
         Categories = [],
         Targets = []
      };
      Console.WriteLine(toReturn.Name);
      return toReturn;
   }


   public async Task<IEnumerable<ProjectsEntity>> UpdateAsync(ProjectsEntity project)
   {
      Console.WriteLine("Updating Projects - Backend(Database)");
     using var db = await dbFactory.CreateConnectionAsync(); 
     await  db.ExecuteAsync("""
                     UPDATE projects_table
                     Set name = @Name, cost = @Cost
                     WHERE id = @id
                     """,
        project);
     return await GetByElectionID(project.ElectionId);
   }

   public async Task<bool> DeleteAsync(Guid projectId)
   {
      Console.WriteLine("Deleting Projects - Backend(Database)");
      using var db = await dbFactory.CreateConnectionAsync();
      var query = await db.ExecuteAsync("""
                                        DELETE FROM projects_table
                                        WHERE id = @project_id
                                        """,new {project_id = projectId});
      if (query != 0)
      {
         Console.WriteLine("Nothing was deleted");
         return false;
      }
      return true;
   }

   public async Task<ProjectsEntity?> GetByIdAsync(Guid projectId)
   {
      _logger.LogInformation("Getting Project with projectId: "+projectId);
     using var db = await dbFactory.CreateConnectionAsync();
     const string queryprojects = """
                          SELECT id as ID , election_id as ElectionId, name as Name, cost as Cost
                          FROM projects_table
                          WHERE id = @project_id
                          """;
     
     var result = await db.QueryAsync<ProjectsEntity>(queryprojects, new {project_id = projectId});
     foreach (var project in result)
     {
        var projectIdGuid = project.Id;
        using var db1 = await dbFactory.CreateConnectionAsync();
        var query = await db1.QueryAsync<int>("""
                                              Select Count(*) as votes
                                              FROM scores_table as s
                                              Where s.project_id = @idAsGuid
                                              GROUP BY  project_id
                                              ORDER BY votes Desc
                                              """,new {idAsGuid = projectIdGuid});
        if (query.Any())
        {
           project.votes = query.ToList().First();
        }
        else
        {
           project.votes = 0;
        }
         
     } 
     return result.FirstOrDefault();
   }
}