using Backend.Repositories;
using Backend.Services.ApiServices.PbEngine;
using Backend.Services.DataServices;
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
            string connectionString = "http://pbengine:8000";
            client.BaseAddress = new Uri(connectionString);
        });
    }

    private static void SetupServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IInitialtest, Initialtest>();
        builder.Services.AddScoped<ElectionService>();
        builder.Services.AddScoped<ProjectService>();

    }

    private static void SetupRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ElectionRepository>();
        builder.Services.AddScoped<ProjectRepository>();
        
    }

}
