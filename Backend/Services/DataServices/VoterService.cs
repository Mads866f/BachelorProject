using AutoMapper;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;

public class VoterService : IVotersService
{
    private readonly IMapper _mapper;
    private readonly IVotersRepository _repository;

    public VoterService(IMapper mapper, IVotersRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<IEnumerable<Voter>> GetAllVotersAsync()
    {
        var result = await _repository.GetAllAsync();
        var voterDto = result
            .Select(x => _mapper.Map<Voter>(x));
        return voterDto;
    }

    public async Task<Voter?> GetVotersAsync(string id)
    {
        var voterEntity = await _repository.GetByIdAsync(Guid.Parse(id));
        var voterDto = _mapper.Map<Voter>(voterEntity);
        return voterDto;
    }

    public async Task<Voter> CreateVotersAsync(CreateVoter voterModel)
    {
        var voterEntity = await _repository.CreateAsync(voterModel);
        var voterDto = _mapper.Map<Voter>(voterEntity);
        return voterDto;
    }

    public async Task<Voter?> UpdateVotersAsync(Voter voterModel)
    {
        var voterEntity = _mapper.Map<VoteEntity>(voterModel);
        var result = await _repository.UpdateAsync(voterEntity);
        return result is null ? _mapper.Map<Voter>(result) : null;
    }

    public async Task<bool> DeleteByIdAsync(string id)
    {
        return await _repository.DeleteAsync(Guid.Parse(id));
    }
}