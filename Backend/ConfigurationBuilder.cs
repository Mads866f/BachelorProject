using Backend.Database;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.ApiServices.PbEngine;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using Backend.Services.Interfaces.PbEngine;
using Backend.Utilities;
using Backend.Utilities.Mappings;

namespace Backend;

public static class ConfigurationBuilder
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        SetupHttpClient(builder);
        SetupMappings(builder);
        SetupRepositories(builder);
        SetupServices(builder);
    }

    private static void SetupMappings(WebApplicationBuilder builder)
    {
        var mapperConfig = AutoMapperConfig.ConfigureMappings();
        builder.Services.AddSingleton(mapperConfig.CreateMapper());
    }

    // Setup http client for PBengine
    private static void SetupHttpClient(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient(Constants.PbEngine, client =>
        {
            //string connectionString = builder.Configuration.GetConnectionString("BackendAPI")
            //                        ?? throw new InvalidOperationException(
            //                          "Connection string 'BackendAPI' not found.");
            // TODO CHANGE FOR DOCKER WHOLE PROJECT
            //string connectionString = "http://pbengine:8000";
            string connectionString = "http://localhost:8000";
            client.BaseAddress = new Uri(connectionString);
        });
    }

    private static void SetupServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IInitialtest, Initialtest>();
        builder.Services.AddScoped<IElectionService, ElectionService>();
        builder.Services.AddScoped<IVotersService, VoterService>();
        builder.Services.AddScoped<IScoresService, ScoresService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IPbEngineService, PbEngineService>();
        builder.Services.AddScoped<ElectionResultService>(); //TODO ADD INTERFACE
        builder.Services.AddSingleton(new GlobalDatabaseSemaphore(1_000_000));
    }

    private static void SetupRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IElectionRepository, ElectionRepository>();
        builder.Services.AddScoped<IVotersRepository, VotersRepository>();
        builder.Services.AddScoped<IScoresRepository, ScoresRepository>();
        builder.Services.AddScoped<IProjectsRepository, ProjectRepository>();
        builder.Services.AddScoped<IElectionResultRepository, ElectionResultRepository>();

    }

}
