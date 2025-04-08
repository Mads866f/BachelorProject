using System.Data;
using Backend.Database;
using Backend.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;

namespace TestsBackend.Database;

public class SqliteDbFactory : IDbConnectionFactory
{
    private  SqliteConnection _connection;
    private readonly SqliteConnection _keeperConnection;
    private readonly string _connectionString = "Data Source=file:memdb1?mode=memory&cache=shared";

    public SqliteDbFactory()
    {
        _keeperConnection = new SqliteConnection(_connectionString);
        _keeperConnection.Open();
        InitializeDatabase();
    }
    public Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        _connection = new SqliteConnection(_connectionString);
        return Task.FromResult((IDbConnection) _connection);
    }

    public void CloseAll()
    {
        _keeperConnection.Dispose();
        _keeperConnection.Close();
    }
    public void InitializeDatabase()
    {
        var tables = new List<string>{ 
            "DROP TABLE IF EXISTS elections_table;",
            "DROP TABLE IF EXISTS projects_table;",
            "DROP TABLE IF EXISTS voters_table;",
            "DROP TABLE IF EXISTS scores_table;",
            "DROP TABLE IF EXISTS categories_table;",
            "DROP TABLE IF EXISTS project_categories_table;",
            "DROP TABLE IF EXISTS targets_table;",
            "DROP TABLE IF EXISTS project_targets_table;",
            """
            CREATE TABLE IF NOT EXISTS elections_table (
                id TEXT PRIMARY KEY ,
                name TEXT NOT NULL,
                total_budget INT NOT NULL,
                model TEXT NOT NULL,
                ballot_design TEXT NOT NULL
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS projects_table (
                id TEXT PRIMARY KEY,
                election_id TEXT REFERENCES elections_table(id),
                name TEXT NOT NULL,
                cost INT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS voters_table (
                id TEXT PRIMARY KEY ,
                election_id TEXT REFERENCES elections_table(id)
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS scores_table (
                voter_id TEXT REFERENCES voters_table(id),
                project_id TEXT REFERENCES projects_table(id),
                grade INT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS categories_table(
                id TEXT PRIMARY KEY ,
                name TEXT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS project_categories_table(
                project_id TEXT REFERENCES projects_table(id),
                category_id TEXT REFERENCES categories_table(id)
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS targets_table(
                id TEXT PRIMARY KEY ,
                name TEXT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS project_targets_table(
                project_id TEXT REFERENCES projects_table(id),
                target_id TEXT REFERENCES targets_table(id)
            )
            """
        };
        foreach (var table in tables)
        {
           using var command = _keeperConnection.CreateCommand();
           command.CommandText = table;
           command.ExecuteNonQuery();
        }
    }
}