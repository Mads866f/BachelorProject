using System.Data;
using Backend.Database;
using Dapper;

namespace Backend;

public static class DatabaseConfigurationBuilder
{
    public static async Task ApplyDatabaseConfiguration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var connectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
        using var db = await connectionFactory.CreateConnectionAsync();
        EnsureDatabaseSetup(db);
    }

    private static void EnsureDatabaseSetup(IDbConnection db)
    {
        // Specify the table's to add
        var tables = new List<string>{ 
            """
            CREATE TABLE IF NOT EXISTS elections (
            id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
            name TEXT NOT NULL,
            total_budget INT NOT NULL,
            model TEXT NOT NULL,
            join_code TEXT NOT NULL UNIQUE,
            ballot_design TEXT NOT NULL
            );
            """};
        // Add the table's to the database
        foreach (var table in tables)
        {
            db.Execute(table);
        }
    }
}