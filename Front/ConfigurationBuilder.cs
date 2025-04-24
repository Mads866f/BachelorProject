using Front.Services.ApiService;
using Front.Services.ApiService.Elections;
using Front.Services.Interface;
using Front.Services.Interface.Elections;
using Front.Utilities;
using Microsoft.JSInterop;

namespace Front;

public static class ConfigurationBuilder
{
    public static void AddConfiguration(this WebApplicationBuilder builder, string connectionString)
    {
        SetupHttpClient(builder, connectionString);
        SetupServices(builder);
    }
    
    // Setup http client for Backend
    private static void SetupHttpClient(WebApplicationBuilder builder,string connection)
    {
        builder.Services.AddHttpClient(Constants.Backend, client =>
        {
            //string connectionString = builder.Configuration.GetConnectionString("BackendAPI")
              //                        ?? throw new InvalidOperationException(
                //                          "Connection string 'BackendAPI' not found.");
            client.BaseAddress = new Uri(connection);
        });
    }

    // Setup the frontend services
    private static void SetupServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IElectionsApiService, ElectionsApiService>();
        builder.Services.AddScoped<IProjectsApiService, ProjectsApiService>();
        builder.Services.AddScoped<IVotersApiService,VotersApiService>();
        builder.Services.AddScoped <IScoresApiService,ScoresApiService>();
        builder.Services.AddScoped<IPbEngineApiService, PbEngineApiService>();
        builder.Services.AddScoped<IElectionResultsApiService, ElectionResultsApiService>();
        builder.Services.AddScoped<Random>();
    }
}
