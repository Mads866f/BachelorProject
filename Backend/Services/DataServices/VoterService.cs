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
    private ILogger<VoterService> _logger;

    public VoterService(IMapper mapper, IVotersRepository repository, IScoresService scoresService, ILogger<VoterService> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }
    public async Task<IEnumerable<Voter>> GetAllVotersAsync()
    {
        var result = await _repository.GetAllAsync();
        _logger.LogInformation($"Getting all voters");
        var voterDtos = result
            .Select(x => _mapper.Map<Voter>(x)).ToList();
        return voterDtos;
    }

    public async Task<IEnumerable<Voter>> GetVotersByElectionId(Guid electionId)
    {
        _logger.LogInformation($"Getting voters for election with id {electionId}");
        var result = await _repository.GetByElectionIdAsync(electionId);
        var voterDtos = result.Select(x => _mapper.Map<Voter>(x)).ToList();
        return voterDtos;
    }

    public async Task<IEnumerable<CoherrentVoter>> GetCoherentVotersFromElection(Guid electionId, int projectCount,int lowerbound)
    {
        _logger.LogInformation($"Get Coherent Voters for election with id {electionId} with {projectCount} projects and at least {lowerbound} number of members");
        var result = await _repository.GetKSizeCoherentVotersFromElection(electionId, projectCount,lowerbound);
        var votersInElection = await _repository.GetByElectionIdAsync(electionId);
        var noOfVotersInElection = votersInElection.Count();

        var transformedResult = result.Select(async pair =>
        {
            var voterList = await _repository.GetVotersWithIdListAsync(pair.Value);
            var voteEntities = voterList.ToList();
            return new CoherrentVoter()
            {
                fraction = (int)(((float)pair.Value.Count()/noOfVotersInElection)*100.0),
                id = Guid.NewGuid(),
                number_of_voters = noOfVotersInElection,
                ShowDetails = false,
                projects = pair.Key,
                voters = _mapper.Map<List<Voter>>(voterList),
                No_In_Group = voteEntities.Count()
            };
        });
        var coherentVoters = await Task.WhenAll(transformedResult);
        return coherentVoters;
    }

    public async Task<Voter?> GetVoterAsync(Guid id)
    {
        _logger.LogInformation($"Getting voter with id {id}");
        var voterEntity = await _repository.GetByIdAsync(id);
        var voterDto = _mapper.Map<Voter>(voterEntity);
        return voterDto;
    }

    public async Task<IEnumerable<Voter>> GetVotersByProjectIdAsync(int projectId)
    {
        _logger.LogInformation($"Getting voters for project with id {projectId}");
        var result = await _repository.GetVotersByProjectIdAsync(projectId);
        var voterDtos = result.Select(x => _mapper.Map<Voter>(x)).ToList();
        return voterDtos;
    }

    public async Task<Voter> CreateVoterAsync(CreateVoter voterModel)
    {
        _logger.LogInformation("Creating new voter");
        var voterEntity = await _repository.CreateAsync(voterModel);
        var voterDto = _mapper.Map<Voter>(voterEntity);
        return voterDto;
    }

    public async Task<Voter?> UpdateVoterAsync(Voter voterModel)
    {
        _logger.LogInformation("Updating voter");
        var voterEntity = _mapper.Map<VoteEntity>(voterModel);
        var result = await _repository.UpdateAsync(voterEntity);
        return result is not null ? _mapper.Map<Voter>(result) : null;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        _logger.LogInformation($"Deleting voter with id {id}");
        return await _repository.DeleteAsync(id);
    }
}