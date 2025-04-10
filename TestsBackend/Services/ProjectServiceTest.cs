using AutoMapper;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using Backend.Utilities.Mappings;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestsBackend.Services;

public class ProjectServiceTest
{
    private readonly IProjectService _service;
    private readonly Mock<IProjectsRepository> _repository;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ProjectService>> _logger;
    
    public ProjectServiceTest()
    {
        //Sets up mock Election Repository
        // Uses Mapper from backend
        _repository = new Mock<IProjectsRepository>();
        var mapperConfig = AutoMapperConfig.ConfigureMappings();
        _mapper = mapperConfig.CreateMapper();
        _logger = new Mock<ILogger<ProjectService>>();
        _service = new ProjectService(_repository.Object,_mapper, _logger.Object);
    }
    
    
}