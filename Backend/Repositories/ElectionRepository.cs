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
        var result =  await db.QueryAsync<ElectionEntity>("SELECT * FROM elections");
        Console.WriteLine("Result pulled from database"+result);
        return result;
    }

    public async Task<ElectionEntity?> GetByIdAsync(Guid id)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        return db.QuerySingleOrDefault<ElectionEntity>(
            """
            SELECT * 
            FROM elections
            WHERE id = @id limit 1
            """, new { id });
    }


    public async Task<ElectionEntity> CreateAsync(ElectionEntity election)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        await db.ExecuteAsync(
            """
            INSERT INTO elections (id, name, total_budget, model, join_code, ballot_design)                 
            Values (@Id, @Name, @TotalBudget, @Model, @JoinCode, @BallotDesign)
            """, election);
        return election;
    }

    public async Task<ElectionEntity> UpdateAsync(ElectionEntity election)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        await db.ExecuteAsync(
            """
            UPDATE elections 
            SET name = @Name, 
                total_budget = @TotalBudget, 
                model = @Model, 
                join_code = @JoinCode, 
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
            DELETE FROM elections 
            WHERE id = @Id
            """, 
            new { Id = id });

        return rowsAffected > 0; // Returns true if at least one row was deleted
    }

}