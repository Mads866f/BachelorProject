using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Backend.Database;
using Backend.Models;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class ProjectRepository(IDbConnectionFactory dbFactory)
{
   public async Task<IEnumerable<ProjectsEntity>> GetByElectionID(string electionID)
   {
      Console.WriteLine("Getting Projects - Backend(Database)");
      using var db = await dbFactory.CreateConnectionAsync();

      var idAsGuid = Guid.Parse(electionID);
      var query = await db.QueryAsync<ProjectsEntity>(new StringBuilder().Append(""" 
                                                           SELECT id as Id, name as Name, election_id as ElectionId, cost as Cost 
                                                           FROM projects_table as p 
                                                           WHERE p.election_id = @idAsGuid
                                                           """)
         .ToString(),new {idAsGuid = idAsGuid});
      
      Console.WriteLine("PROJECTS PULLED FROM DB:"+query.ToString());
      
      return query;
   }

   public async Task<Project> CreateAsync(ProjectsEntity project)
   {
      using var db = await dbFactory.CreateConnectionAsync();
      const string query = """
                           INSERT INTO projects_table (election_id, name, cost)
                           Values (@ElectionID, @Name, @Cost)
                           RETURNING id
                           """;
      var projectId = await db.QuerySingleAsync<Guid>(query, project);
      var toReturn =  new Project
      {
         Id = projectId,
         ElectionId = project.ElectionID,
         Name = project.Name,
         Cost = project.Cost,
         categories = [],
         targets = []
      };
      Console.WriteLine(toReturn.Name);
      return toReturn;
   }
}