using Backend;
using Backend.Database;
using Microsoft.AspNetCore.DataProtection;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"keys"));

// Get Connection string
// TODO CHANGE CONNECTION STRING TO MAKE IT WORK IN DOCKER CONTAINER
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

string pbconnection = "";
if(args.Contains("-Dev"))
{ 
    connectionString = builder.Configuration.GetConnectionString("Local_Postgres_db");

    pbconnection= "http://localhost:8000";
}
else{
    connectionString = builder.Configuration.GetConnectionString("Postgres_db");
    pbconnection= "http://pbengine:8000";
}
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlDbConnectionFactory(connectionString!));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // This ensures that summary and response type information is included in Swagger UI
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Backend.xml"));
});

builder.AddConfiguration(pbconnection);
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.ApplyDatabaseConfiguration();

app.MapGet("/", () => "Dapper API is running!");

app.UseHttpsRedirection();

app.Run();
