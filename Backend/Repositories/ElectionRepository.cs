using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Dapper;
using DTO.Models;


namespace Backend.Repositories;

public class ElectionRepository(IDbConnectionFactory dbFactory) : IElectionRepository
{
    public async Task<IEnumerable<ElectionEntity>> GetAllAsync()
    {
        using var db = await dbFactory.CreateConnectionAsync();
        var result = await db.QueryAsync<ElectionEntity>(
            """
            SELECT id, name, total_budget AS TotalBudget, model, ballot_design AS BallotDesign 
            FROM elections_table
            """);
        return result;
    }

    public async Task<ElectionEntity?> GetByIdAsync(Guid id)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        return db.QuerySingleOrDefault<ElectionEntity>(
            """
            SELECT id, name, total_budget AS TotalBudget, model, ballot_design AS BallotDesign 
            FROM elections_table
            WHERE id = @idToLookUp LIMIT 1
            """, new { idToLookUp = id });
    }


    public async Task<ElectionEntity> CreateAsync(CreateElectionModel election)
    {
        using var db = await dbFactory.CreateConnectionAsync();

        const string query =
            """
            INSERT INTO elections_table (name, total_budget, model, ballot_design)                 
            VALUES (@Name, @TotalBudget, @Model, @BallotDesign)
            RETURNING id;
            """;
        var electionId = await db.QuerySingleAsync<Guid>(query, election);

        return new ElectionEntity
        {
            Id = electionId,
            Name = election.Name,
            TotalBudget = election.TotalBudget,
            Model = election.Model,
            BallotDesign = election.BallotDesign
        };
    }

    public async Task<ElectionEntity?> UpdateAsync(ElectionEntity election)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        await db.ExecuteAsync(
            """
            UPDATE elections_table 
            SET name = @Name, 
                total_budget = @TotalBudget, 
                model = @Model, 
                ballot_design = @BallotDesign
            WHERE id = @Voter_Id
            """,
            election);
        return await GetByIdAsync(election.Id) ?? null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        var rowsAffected = await db.ExecuteAsync(
            """
            DELETE 
            FROM elections_table 
            WHERE id = @Voter_Id
            """,
            new { Id = id });
        if (rowsAffected != 0) return true;
        Console.WriteLine($"Warning: Attempted to delete non-existing election with Voter_Id {id}", id);
        return false;
    }
}