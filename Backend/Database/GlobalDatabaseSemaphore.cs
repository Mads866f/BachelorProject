namespace Backend.Database;

public class GlobalDatabaseSemaphore
{
    public SemaphoreSlim semaphore { get; }

    public GlobalDatabaseSemaphore(int maxConcurrency)
    {
        semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
    }
    
}