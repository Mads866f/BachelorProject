using AutoMapper;
using Backend.Database;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services.DataServices;

public class ScoresService : IScoresService
{
    private readonly IMapper _mapper;
    private readonly IScoresRepository _repository;
    private readonly IVotersRepository _votersRepository;
    private readonly IProjectsRepository _projectsRepository;
    private readonly GlobalDatabaseSemaphore _semaphore;

    public ScoresService(IMapper mapper, IScoresRepository repository,IVotersRepository votersRepository, IProjectsRepository projectsRepository, GlobalDatabaseSemaphore sem)
    {
        _mapper = mapper;
        _repository = repository;
        _votersRepository = votersRepository;
        _projectsRepository = projectsRepository;
        _semaphore = sem;
    }

    public async Task<IEnumerable<Scores>> GetScoresForVoterIdAsync(Guid id)
    {
        await _semaphore.semaphore.WaitAsync();
        IEnumerable<Scores> scoresDto;
        try
        {
            //Check If Voter Exists
            var voter = await _votersRepository.GetByIdAsync(id);
            var voterExists = voter is not null;
            if (!voterExists)
            {
                return (List<Scores>)null;
            }

            //Get Votes
            var result = await _repository.GetScoreForVoter(id);
            scoresDto = result
                .Select(x => _mapper.Map<Scores>(x));
            //Add Projects
            scoresDto.Select(async s => s.project = _mapper.Map<Project>(await _projectsRepository.GetByIdAsync(s.Project_Id)));
        }
        finally
        {
            _semaphore.semaphore.Release();
        }
        return scoresDto;

    }

    public async Task<IEnumerable<Scores>> GetScoresForProjectIdAsync(Guid id)
    {
        var result = await _repository.GetScoreForProject(id);
        var scoresDto = result
            .Select(x => _mapper.Map<Scores>(x));
        return scoresDto;
    }

    public async Task<Scores?> CreateVotersAsync(Scores voterModel)
    {
        var voterExists = await _votersRepository.GetByIdAsync(voterModel.Voter_Id) is null;
        var projectExists = await _projectsRepository.GetByIdAsync(voterModel.Project_Id) is null;
        if (voterExists || projectExists)
        {
            return null;
        }
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

    public async Task<bool> DeleteByIdAsync(Guid voterId, Guid projectId)
    {
        return await _repository.DeleteAsync(voterId, projectId);
    }
}