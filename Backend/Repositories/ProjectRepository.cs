using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class ProjectRepository(IDbConnectionFactory dbFactory) : IProjectsRepository
{
   public async Task<IEnumerable<ProjectsEntity>> GetByElectionID(Guid electionID)
   {
      Console.WriteLine("Getting Projects - Backend(Database)");
      using var db = await dbFactory.CreateConnectionAsync();

      var query = await db.QueryAsync<ProjectsEntity>(new StringBuilder().Append(""" 
                                                           SELECT id as Id, name as Name, election_id as ElectionId, cost as Cost 
                                                           FROM projects_table as p 
                                                           WHERE p.election_id = @idAsGuid
                                                           """)
         .ToString(),new {idAsGuid = electionID});
      
      Console.WriteLine("PROJECTS PULLED FROM DB:"+query.ToString());
      
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
         categories = [],
         targets = []
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

   public async Task<bool> DeleteAsync(Guid project_id)
   {
      Console.WriteLine("Deleting Projects - Backend(Database)");
      using var db = await dbFactory.CreateConnectionAsync();
      var query = await db.ExecuteAsync("""
                                        DELETE FROM projects_table
                                        WHERE id = @project_id
                                        """,new {project_id = project_id});
      if (query != 0)
      {
         Console.WriteLine("Nothing was deleted");
         return false;
      }
      return true;
   }

   public async Task<ProjectsEntity?> GetByIdAsync(Guid projectId)
   {
     Console.Write("Getting Projects - Backend(Database)");
     using var db = await dbFactory.CreateConnectionAsync();
     const string query = """
                          SELECT id as ID , election_id as ElectionId, name as Name, cost as Cost
                          FROM projects_table
                          WHERE id = @project_id
                          """;
     var result = await db.QueryAsync<ProjectsEntity>(query, new {project_id = projectId});
     return result.FirstOrDefault();
   }
}