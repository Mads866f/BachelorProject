using Frontend.Services;
using Frontend.Services.ApiService;
using Frontend.Utilities;

namespace Frontend;

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
            string connectionString = builder.Configuration.GetConnectionString("BackendAPI")
                                      ?? throw new InvalidOperationException(
                                          "Connection string 'BackendAPI' not found.");
            client.BaseAddress = new Uri(connectionString);
        });
    }

    // Setup the frontend services
    private static void SetupServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserApiService, UserApiService>();
    }
}