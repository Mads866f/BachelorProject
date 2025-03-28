using AutoMapper;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;

namespace Backend.Services.DataServices;

public class ScoresService : IScoresService
{
    private readonly IMapper _mapper;
    private readonly IScoresRepository _repository;

    public ScoresService(IMapper mapper, IScoresRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<Scores>> GetScoresForVoterIdAsync(string id)
    {
        var result = await _repository.GetScoreForVoter(Guid.Parse(id));
        var scoresDto = result
            .Select(x => _mapper.Map<Scores>(x));
        return scoresDto;

    }

    public async Task<IEnumerable<Scores>> GetScoresForProjectIdAsync(string id)
    {
        var result = await _repository.GetScoreForProject(Guid.Parse(id));
        var scoresDto = result
            .Select(x => _mapper.Map<Scores>(x));
        return scoresDto;
    }

    public async Task<Scores> CreateVotersAsync(Scores voterModel)
    {
        var result = await _repository.CreateAsync(voterModel);
        var scoresDto = _mapper.Map<Scores>(result);
        return scoresDto;
    }

    public async Task<Scores?> UpdateVotersAsync(Scores voterModel)
    {
        var scoresEntity = _mapper.Map<ScoresEntity>(voterModel);
        var result = await _repository.UpdateAsync(scoresEntity);
        return result is not null ? _mapper.Map<Scores>(result) : null;
    }

    public async Task<bool> DeleteByIdAsync(string voterId, string projectId)
    {
        return await _repository.DeleteAsync(Guid.Parse(voterId), Guid.Parse(projectId));
    }
}