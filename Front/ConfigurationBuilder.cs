using Front.Services;
using Front.Services.ApiService;
using Front.Services.ApiService.Elections;
using Front.Services.Elections;
using Front.Utilities;

namespace Front;

public static class ConfigurationBuilder
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        SetupHttpClient(builder);
        SetupServices(builder);
    }
    
    // Setup http client for Backend
    private static void SetupHttpClient(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient(Constants.Backend, client =>
        {
            //string connectionString = builder.Configuration.GetConnectionString("BackendAPI")
              //                        ?? throw new InvalidOperationException(
                //                          "Connection string 'BackendAPI' not found.");
            string connectionString =  "http://backend:8080";
            client.BaseAddress = new Uri(connectionString);
        });
    }

    // Setup the frontend services
    private static void SetupServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ElectionsApiService>();
        builder.Services.AddScoped<ProjectsApiService>();
    }
}