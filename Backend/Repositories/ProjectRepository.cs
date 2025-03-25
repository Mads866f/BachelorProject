using System.Text;
using Backend.Database;
using Backend.Models;
using Dapper;

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
}