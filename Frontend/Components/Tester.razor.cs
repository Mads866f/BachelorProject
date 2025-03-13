using Frontend.Services.ApiService;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Query;

namespace Frontend.Components;

public partial class Tester :ComponentBase
{
    [Inject] public required TestApiService TestApiService { get; set; }

    public string das;

    protected override async void OnInitialized()
    {
        base.OnInitialized();
        das = await TestApiService.Test();
        Console.WriteLine("DAS: "+das);
    }
}