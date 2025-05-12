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

    public VoterService(IMapper mapper, IVotersRepository repository, IScoresService scoresService)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<IEnumerable<Voter>> GetAllVotersAsync()
    {
        var result = await _repository.GetAllAsync();
        var voterDtos = result
            .Select(x => _mapper.Map<Voter>(x)).ToList();
        return voterDtos;
    }

    public async Task<IEnumerable<Voter>> GetVotersByElectionId(Guid electionId)
    {
        var result = await _repository.GetByElectionIdAsync(electionId);
        var voterDtos = result.Select(x => _mapper.Map<Voter>(x)).ToList();
        return voterDtos;
    }

    public async Task<IEnumerable<CoherrentVoter>> GetCoherentVotersFromElection(Guid electionId, int projectCount,int lowerbound)
    {
        var result = await _repository.GetKSizeCoherentVotersFromElection(electionId, projectCount,lowerbound);
        var votersInElection = await _repository.GetByElectionIdAsync(electionId);
        var noOfVotersInElection = votersInElection.Count();

        var transformedResult = result.Select(async pair =>
        {
            var voterList = await _repository.GetVotersWithIdListAsync(pair.Value);
            return new CoherrentVoter()
            {
                fraction = (int)((float)pair.Value.Count() / noOfVotersInElection),
                id = Guid.NewGuid(),
                number_of_voters = noOfVotersInElection,
                ShowDetails = false,
                projects = pair.Key,
                voters = _mapper.Map<List<Voter>>(voterList),

            };
        });
        var coherentVoters = await Task.WhenAll(transformedResult);
        return coherentVoters;
    }

    public async Task<Voter?> GetVoterAsync(Guid id)
    {
        var voterEntity = await _repository.GetByIdAsync(id);
        var voterDto = _mapper.Map<Voter>(voterEntity);
        return voterDto;
    }

    public async Task<IEnumerable<Voter>> GetVotersByProjectIdAsync(int projectId)
    {
        var result = await _repository.GetVotersByProjectIdAsync(projectId);
        var voterDtos = result.Select(x => _mapper.Map<Voter>(x)).ToList();
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
}