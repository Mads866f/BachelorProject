using Backend.Services.ApiServices.PbEngine;
using Backend.Services.Interfaces.PbEngine;
using Backend.Utilities;

namespace Backend;

public static class ConfigurationBuilder
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        SetupHttpClient(builder);
        SetupServices(builder);
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
    }
}
