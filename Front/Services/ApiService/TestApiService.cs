using BlazorApp1.Utilities;

namespace BlazorApp1.Services.ApiService;

public class TestApiService(IHttpClientFactory clientFactory): ITestApiService
{
    private readonly HttpClient _httpClient =  clientFactory.CreateClient(Constants.Backend);
    
    public async Task<string> Test()
    {
        var url = "api/Test";
        try
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return "DSADSADSA";
        }
        catch (Exception e)
        {
            return "FAILED";
        }
    }
}