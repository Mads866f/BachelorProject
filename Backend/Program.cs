using System.Data;
using Backend;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Get Connection string
var connectionString = builder.Configuration.GetConnectionString("Postgres_db");

builder.Services.AddSingleton<IDbConnection>(sp => new NpgsqlConnection(connectionString));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddConfiguration();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Ensure database and tables are set up
void EnsureDatabaseSetup(IDbConnection db)
{
    db.Open();
    using var cmd = db.CreateCommand();
    cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS users (
            id SERIAL PRIMARY KEY,
            name TEXT NOT NULL
        )";
    cmd.ExecuteNonQuery();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    Console.WriteLine("IM AM HERE");
    EnsureDatabaseSetup(db);
}

app.MapGet("/", () => "Dapper API is running!");

app.UseHttpsRedirection();

app.Run();
