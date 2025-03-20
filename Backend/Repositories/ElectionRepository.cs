using Backend.Database;
using Backend.Models;
using Dapper;


namespace Backend.Repositories;

public class ElectionRepository(IDbConnectionFactory dbFactory)
{
    public async Task<IEnumerable<ElectionEntity>> GetAllAsync()
    {
        Console.WriteLine("Started Getting from database");
        using var db = await dbFactory.CreateConnectionAsync();
        Console.WriteLine("Connnected to Database");
        var result =  await db.QueryAsync<ElectionEntity>("SELECT * FROM elections_table");
        Console.WriteLine("Result pulled from database"+result);
        return result;
    }

    public async Task<ElectionEntity?> GetByIdAsync(Guid id)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        
        var a = db.QuerySingleOrDefault<ElectionEntity>(
            """
            SELECT id, id, name, total_budget AS TotalBudget, model, ballot_design AS BallotDesign 
            FROM elections_table
            WHERE id = @id limit 1
            """, new { id });
        Console.WriteLine(a.TotalBudget + "DIRECT FROM DB");
        return a;
    }


    public async Task<ElectionEntity> CreateAsync(ElectionEntity election)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        await db.ExecuteAsync(
            """
            INSERT INTO elections_table (id, name, total_budget, model, ballot_design)                 
            Values (@Id, @Name, @TotalBudget, @Model, @BallotDesign)
            """, election);
        return election;
    }

    public async Task<ElectionEntity> UpdateAsync(ElectionEntity election)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        await db.ExecuteAsync(
            """
            UPDATE elections_table 
            SET name = @Name, 
                total_budget = @TotalBudget, 
                model = @Model, 
                ballot_design = @BallotDesign
            WHERE id = @Id
            """,
            election);
        return election;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        var rowsAffected = await db.ExecuteAsync(
            """
            DELETE FROM elections_table 
            WHERE id = @Id
            """, 
            new { Id = id });

        return rowsAffected > 0; // Returns true if at least one row was deleted
    }

}