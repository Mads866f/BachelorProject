using AutoMapper;
using Backend.Repositories.Interfaces;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using Backend.Utilities.Mappings;
using Moq;

namespace TestBackend.Services;

public class ElectionServiceTest
{
        
    private readonly IElectionService _service;
    private readonly Mock<IElectionRepository> _repository;

    public ElectionServiceTest()
    {
        //Sets up mock Election Repository
        // Uses Mapper from backend
        _repository = new Mock<IElectionRepository>();
        var mapperConfig = AutoMapperConfig.ConfigureMappings();
        var mapper = mapperConfig.CreateMapper();
        _service = new ElectionService(mapper,_repository.Object);
    }
    
    [Fact]
    
}