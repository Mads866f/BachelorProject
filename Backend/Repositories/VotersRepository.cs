using System.Runtime.InteropServices;
using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Dapper;
using DTO.Models;

namespace Backend.Repositories;

public class VotersRepository(IDbConnectionFactory dbFactory) : IVotersRepository
{
    private IVotersRepository _votersRepositoryImplementation;

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
        Console.WriteLine("Getting Voter with id:" + id);
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             SELECT id AS Id, election_id AS ElectionId
                             FROM voters_table
                             WHERE id = @idToFind LIMIT 1
                             """;
        var result =db.QuerySingleOrDefault<VoteEntity>(query, new {idToFind = id});
        if (result is not null)
        {
            Console.WriteLine("Found a result with id:" + result.Id);
        }
        return result;
    }

    public async Task<VoteEntity> CreateAsync(CreateVoter voter)
    {
        Console.WriteLine("Creating voter for election: " + voter.ElectionId);
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
        Console.WriteLine("Updating voter: " + voter.Id);
        using var db = await dbFactory.CreateConnectionAsync();
    
        const string query = """
                             UPDATE voters_table
                             SET election_id = @ElectionId
                             WHERE id = @Id
                             """;
        await db.ExecuteAsync(query, voter);
        return await GetByIdAsync(voter.Id) ?? null;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        Console.WriteLine("Deleting voter: " + id);
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             DELETE FROM voters_table
                             WHERE id = @Id
                             """;
        var rowsAffected = await db.ExecuteAsync(query, new {Id = id});
        if (rowsAffected != 0) return true;
        Console.WriteLine($"Warning: Attempted to delete non-existing voter with Id {id}",id);
        return false;
    }

    public async Task<IEnumerable<VoteEntity>> GetByElectionIdAsync(Guid electionId)
    {
        using var db = await dbFactory.CreateConnectionAsync();
        const string query = """
                             SELECT id as Id, election_id as ElectionId 
                             FROM voters_table
                             Where election_id = @ElectionId
                             """;
        var result = await db.QueryAsync<VoteEntity>(query, new {ElectionId = electionId});
        return result;
        
    }

    public Task<IEnumerable<VoteEntity>> GetVotersByProjectIdAsync(int projectId)
    {
        throw new NotImplementedException();
    }
}