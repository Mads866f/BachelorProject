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
    private readonly IScoresService _scoresService;

    public VoterService(IMapper mapper, IVotersRepository repository, IScoresService scoresService)
    {
        _mapper = mapper;
        _repository = repository;
        _scoresService = scoresService;
    }
    public async Task<IEnumerable<Voter>> GetAllVotersAsync()
    {
        var result = await _repository.GetAllAsync();
        var voterDtos = result
            .Select(x => _mapper.Map<Voter>(x)).ToList();
        await Task.WhenAll(voterDtos.Select(async x => await AddScores(x)));
        return voterDtos;
    }

    public async Task<IEnumerable<Voter>> GetVotersByElectionId(Guid electionId)
    {
        var result = await _repository.GetByElectionIdAsync(electionId);
        var voterDtos = result.Select(x => _mapper.Map<Voter>(x)).ToList();
        await Task.WhenAll(voterDtos.Select(async x => await AddScores(x)));
        return voterDtos;
    }

    public async Task<Voter?> GetVoterAsync(string id)
    {
        var voterEntity = await _repository.GetByIdAsync(Guid.Parse(id));
        var voterDto = _mapper.Map<Voter>(voterEntity);
        await AddScores(voterDto);
        return voterDto;
    }

    public async Task<IEnumerable<Voter>> GetVotersByProjectIdAsync(int projectId)
    {
        var result = await _repository.GetVotersByProjectIdAsync(projectId);
        var voterDtos = result.Select(x => _mapper.Map<Voter>(x)).ToList();
        await Task.WhenAll(voterDtos.Select(async x => await AddScores(x)));
        return voterDtos;
    }

    public async Task<Voter> CreateVoterAsync(CreateVoter voterModel)
    {
        var voterEntity = await _repository.CreateAsync(voterModel);
        var voterDto = _mapper.Map<Voter>(voterEntity);
        return voterDto;
    }

    public async Task<Voter?> UpdateVoterAsync(Voter voterModel)
    {
        var voterEntity = _mapper.Map<VoteEntity>(voterModel);
        var result = await _repository.UpdateAsync(voterEntity);
        return result is not null ? _mapper.Map<Voter>(result) : null;
    }

    public async Task<bool> DeleteByIdAsync(string id)
    {
        return await _repository.DeleteAsync(Guid.Parse(id));
    }

    private async Task AddScores(Voter voter)
    {
        var scores = await _scoresService.GetScoresForVoterIdAsync(voter.Id);
        voter.Votes = scores.ToList();
    }
}