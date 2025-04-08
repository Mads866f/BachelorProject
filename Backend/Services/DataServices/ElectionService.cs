using AutoMapper;
using Backend.Models;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;

public class ElectionService : IElectionService
{
    private readonly IMapper _mapper;
    private readonly IElectionRepository _repository;

    public ElectionService(IMapper mapper, IElectionRepository repository)
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
    
    public async Task<Election?> GetElectionAsync(Guid id)
    {
        var electionEntity = await _repository.GetByIdAsync(id);
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
        return result is not null ? _mapper.Map<Election>(result) : null;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}