using BlazorApp1.Utilities;

namespace BlazorApp1.Services.ApiService;

public class UserApiService(IHttpClientFactory clientFactory) : IUserApiService
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient(Constants.Backend);

    public async Task<string> GetUsersAsync()
    {
        var url = "api/User/";
        try
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return "";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}