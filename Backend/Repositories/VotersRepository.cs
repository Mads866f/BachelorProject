using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class VotersRepository(IDbConnectionFactory dbFactory) : IVotersRepository
{
    public async Task<IEnumerable<VoteEntity>> GetAllAsync()
    {
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             SELECT id AS Id, election_id AS ElectionId
                             FROM voters_table
                             """;
        var result = await db.QueryAsync<VoteEntity>(query);
        return result;
    }

    public async Task<VoteEntity?> GetByIdAsync(Guid id)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             SELECT id AS Id, election_id AS ElectionId
                             FROM voters_table
                             WHERE id = @id LIMIT 1
                             """;
        return db.QuerySingleOrDefault<VoteEntity>(query);
    }

    public async Task<VoteEntity> CreateAsync(CreateVoter voter)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             INSERT INTO voters_table (id, election_id)
                             VALUES (@Id, @ElectionId)
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
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             UPDATE voters_table
                             SET id = @Id,
                                 election_id = @ElectionId
                             """;
        await db.ExecuteAsync(query, voter);
        return await GetByIdAsync(voter.Id) ?? null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             DELETE FROM voters_table
                             WHERE id = @id
                             """;
        var rowsAffected = await db.ExecuteAsync(query, id);
        if (rowsAffected != 0) return true;
        Console.WriteLine($"Warning: Attempted to delete non-existing voter with Id {id}",id);
        return false;
    }
}