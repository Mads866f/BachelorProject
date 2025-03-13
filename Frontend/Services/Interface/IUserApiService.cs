namespace Frontend.Services;

public interface IUserApiService
{
    Task<string> GetUsersAsync();
}