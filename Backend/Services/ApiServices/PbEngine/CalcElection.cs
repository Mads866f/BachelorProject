namespace Backend.Services.ApiServices.PbEngine

public class CalcElection(IHttpsClientFactory clientFactory) : ICalcElection{

private readonly HttpClient _httpClient = clientFactory.CreateClient(Constants.PbEngine);


public async Task<string> CalculateElection(string instance,string profile){
    var url = "/calculateElection";
    try
    {
        var response = await _httpClient.GetAsync(url+instance+profile);
        if (response.IsSuccessStatusCode){
            return await response.Content.ReadAsStringAsync();
        }
        return "DID NOT GET ANYTING"
    }
    catch (System.Exception)
    {
        Console.WriteLine(Error Occured);
        throw;
    }

}

}