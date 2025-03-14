namespace BlazorApp1.Services;

public interface IUserApiService
{
    Task<string> GetUsersAsync(); 
}