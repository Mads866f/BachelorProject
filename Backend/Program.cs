using System.Data;
using Backend;
using Backend.Database;
using Backend.Repositories;
using Backend.Services.DataServices;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Get Connection string
var connectionString = builder.Configuration.GetConnectionString("Postgres_db");
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlDbConnectionFactory(connectionString!));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddConfiguration();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.ApplyDatabaseConfiguration();

app.MapGet("/", () => "Dapper API is running!");

app.UseHttpsRedirection();

app.Run();
