namespace Front.Services;

public interface IUserApiService
{
    Task<string> GetUsersAsync(); 
}