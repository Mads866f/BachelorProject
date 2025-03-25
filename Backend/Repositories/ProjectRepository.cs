using System.Text;
using Backend.Database;
using Backend.Models;
using Dapper;

namespace Backend.Repositories;

public class ProjectRepository(IDbConnectionFactory dbFactory)
{
   public async Task<IEnumerable<ProjectsEntity>> GetByElectionID(string electionID)
   {
      using var db = await dbFactory.CreateConnectionAsync();

      var query = await db.QueryAsync<ProjectsEntity>(new StringBuilder().Append(""" 
                                                           SELECT id as Id, name as Name, election_id as ElectionId, cost as Cost 
                                                           FROM projects_table as p 
                                                           WHERE p.election_id = @electionID 
                                                           """)
         .ToString(),new {electionID});
      
      Console.WriteLine("PROJECTS PULLED FROM DB:"+query.ToString());
      
      return query;
   } 
}