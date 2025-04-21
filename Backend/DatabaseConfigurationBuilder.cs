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
            CREATE TABLE IF NOT EXISTS elections_table (
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                name TEXT NOT NULL,
                total_budget INT NOT NULL,
                model TEXT NOT NULL,
                ballot_design TEXT NOT NULL,
                ended BOOLEAN NOT NULL DEFAULT FALSE
            );
            """,
            """
            CREATE TABLE IF NOT EXISTS projects_table (
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                election_id UUID REFERENCES elections_table(id),
                name TEXT NOT NULL,
                cost INT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS voters_table (
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                election_id UUID REFERENCES elections_table(id)
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS scores_table (
                voter_id UUID REFERENCES voters_table(id),
                project_id UUID REFERENCES projects_table(id),
                grade INT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS categories_table(
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                name TEXT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS project_categories_table(
                project_id UUID REFERENCES projects_table(id),
                category_id UUID REFERENCES categories_table(id)
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS targets_table(
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                name TEXT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS project_targets_table(
                project_id UUID REFERENCES projects_table(id),
                target_id UUID REFERENCES targets_table(id)
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS result_table(
                id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                election_id UUID REFERENCES elections_table(id),
                method_used TEXT NOT NULL,
                ballot_used TEXT NOT NULL
            )
            """,
            """
            CREATE TABLE IF NOT EXISTS elected_projects_table(
                result_id UUID REFERENCES result_table(id),
                project_id UUID REFERENCES projects_table(id)
            )
            """
        };
        // Add the table's to the database
        foreach (var table in tables)
        {
            db.Execute(table);
        }
    }
}