using AutoMapper;
using Backend.Models;
using Backend.Repositories;
using DTO.Models;

namespace Backend.Services.DataServices;

public class ElectionService
{
    private readonly IMapper _mapper;
    private readonly ElectionRepository _repository;

    public ElectionService(IMapper mapper, ElectionRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<Election>> GetAllElectionsAsync()
    {
    var result = await _repository.GetAllAsync();
    var electionsDto = result
        .Select(x => _mapper.Map<Election>(x)); 
    return electionsDto;
    }
    
    public async Task<Election?> GetElectionAsync(string id)
    {
        var electionEntity = await _repository.GetByIdAsync(Guid.Parse(id));
        var electionDto = _mapper.Map<Election>(electionEntity);
        return electionDto;
    }

    public async Task<Election> CreateElectionAsync(CreateElectionModel election)
    {
        var electionEntity = await _repository.CreateAsync(election);
        var electionDto = _mapper.Map<Election>(electionEntity);
        return electionDto;
    }

    public async Task<Election?> UpdateElectionAsync(Election electionModel)
    {
        var electionEntity = _mapper.Map<ElectionEntity>(electionModel);
        var result = await _repository.UpdateAsync(electionEntity);
        return result is null ? _mapper.Map<Election>(result) : null;
    }

    public async Task<bool> DeleteByIdAsync(string id)
    {
        return await _repository.DeleteAsync(Guid.Parse(id));
    }
}