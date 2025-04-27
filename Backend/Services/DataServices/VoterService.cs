using AutoMapper;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;
using Front.Components.ResultPage.CoherrentVoter;

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

    public async Task<IEnumerable<CoherrentVoter>> GetCoherentVotersFromElection(Guid electionId, int projectCount,int lowerbound)
    {
        var result = await _repository.GetKSizeCoherentVotersFromElection(electionId, projectCount,lowerbound);
        var votersInElection = await _repository.GetByElectionIdAsync(electionId);
        var noOfVotersInElection = votersInElection.Count();
        var coheretDtos = result.Select(c => new CoherrentVoter()
        {
            fraction = (int)(((float)c.Item2/noOfVotersInElection)*100),
            id = Guid.NewGuid(),
            number_of_voters = c.Item2,
            ShowDetails = false,
            projects = c.Item1.Select(p => _mapper.Map<Project>(p)).ToList()
        });
        return coheretDtos;
    }

    public async Task<Voter?> GetVoterAsync(Guid id)
    {
        var voterEntity = await _repository.GetByIdAsync(id);
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

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private async Task AddScores(Voter voter)
    {
        var scores = await _scoresService.GetScoresForVoterIdAsync(voter.Id);
        voter.Votes = scores.ToList();
    }
}