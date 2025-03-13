using Backend.Services.Interfaces.PbEngine;
using Backend.Utilities;

namespace Backend.Services.ApiServices.PbEngine;

public class Initialtest(IHttpClientFactory clientFactory) : IInitialtest
{
    private readonly HttpClient _httpsClient = clientFactory.CreateClient(Constants.PbEngine);
    
    public async Task<string> Test()
    {
        var url = "/test";
        try
        {
            var response = await _httpsClient.GetAsync(url);
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