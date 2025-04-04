using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using Backend.Utilities.Mappings;
using DTO.Models;
using Moq;

namespace TestsBackend.Services;



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
    public async Task GetAllElectionsAsync_NoParameter_ReturnListOfElections()
    {
        //Arrange
        List<ElectionEntity> listToReturn = [new() { Name = "SOME-NAME" }];
        _repository.Setup(x => x.GetAllAsync()).ReturnsAsync(listToReturn);
        //Act
        var result = await _service.GetAllElectionsAsync();
        //Assert
        result = result.ToList();
        Assert.Single(result);
        Assert.Equal(result.First().Name, listToReturn.First().Name);
        Assert.IsType<List<Election>>(result);
    }
}