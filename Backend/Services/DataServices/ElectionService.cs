using Backend.Models;
using Backend.Repositories;

namespace Backend.Services.DataServices;

public class ElectionService(ElectionRepository repository)
{
    private readonly ElectionRepository _repository = repository;

    public Task<IEnumerable<ElectionEntity>> GetAllElectionsAsync() => _repository.GetAllAsync();

    public Task<ElectionEntity> CreateElectionAsync(ElectionEntity election)
    {
        Console.WriteLine("Created Entrance In Database");
        election.Id = Guid.NewGuid();
        return _repository.CreateAsync(election);
    }
}