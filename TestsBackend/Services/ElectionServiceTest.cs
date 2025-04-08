using AutoMapper;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.DataServices;
using Backend.Services.Interfaces;
using Backend.Utilities.Mappings;
using DTO.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestsBackend.Services;

public class ElectionServiceTest
{
        
    private readonly IElectionService _service;
    private readonly Mock<IElectionRepository> _repository;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ElectionService>> _logger;

    public ElectionServiceTest()
    {
        //Sets up mock Election Repository
        // Uses Mapper from backend
        _repository = new Mock<IElectionRepository>();
        var mapperConfig = AutoMapperConfig.ConfigureMappings();
        _mapper = mapperConfig.CreateMapper();
        _logger = new Mock<ILogger<ElectionService>>();
        _service = new ElectionService(_mapper,_repository.Object, _logger.Object);
    }

    [Fact]
    public async Task GetAllElectionsAsync_NoParameter_ReturnListOfElections()
    {
        //Arrange
        List<ElectionEntity> listToReturn = [new() { Name = "SOME-NAME" , BallotDesign = "1-approval",Id = Guid.Empty, Model = "EqualShares", TotalBudget = 10}];
        _repository.Setup(x => x.GetAllAsync()).ReturnsAsync(listToReturn);
        //Act
        var result = await _service.GetAllElectionsAsync();
        //Assert
        result = result.ToList();
        Assert.Single(result);
        //Checking Translation is good
        Assert.Equal(result.First().Name, listToReturn.First().Name);
        Assert.Equal(result.First().BallotDesign, listToReturn.First().BallotDesign);
        Assert.Equal(result.First().Id, listToReturn.First().Id);
        Assert.Equal(result.First().Model, listToReturn.First().Model);
        Assert.Equal(result.First().TotalBudget, listToReturn.First().TotalBudget);
        //Checking type is good
        Assert.IsType<List<Election>>(result);
    }

    [Fact]
    public async Task GetAllElectionsAsync_NoParameter_ReturnEmptyList()
    {
        //Arrange
        var listToReturn = new List<ElectionEntity>();
        _repository.Setup(x => x.GetAllAsync()).ReturnsAsync(listToReturn);
        //Act
        var result = await _service.GetAllElectionsAsync();
        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_IdExist_ReturnElection()
    {
        //Arrange
        var electionToReturn = new ElectionEntity(){BallotDesign = "1-Approval",Id = Guid.NewGuid(), Model = "EqualShares", TotalBudget = 10};
        _repository.Setup(x => x.GetByIdAsync(electionToReturn.Id)).ReturnsAsync(electionToReturn);
        //Act 
        var result = await _service.GetElectionAsync(electionToReturn.Id);
        //Assert
        Assert.Equal(electionToReturn.Id, result?.Id);
        Assert.Equal(electionToReturn.BallotDesign, result?.BallotDesign);
        Assert.Equal(electionToReturn.Model, result?.Model);
        Assert.Equal(electionToReturn.TotalBudget, result?.TotalBudget);
    }

    [Fact]
    public async Task GetByIdAsync_IdNotExist_ReturnNull()
    {
        //Arrange
        _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ElectionEntity)null);
        //Act
        var result = await _service.GetElectionAsync(Guid.NewGuid());
        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateElectionAsync_NewElection_ReturnElection()
    {
        //Arrange
        var newElection = new CreateElectionModel(){BallotDesign = "1-approval",Model = "EqualShares",Name="Test Election",TotalBudget = 10};
        var electionToReturn = new ElectionEntity() {BallotDesign = "1-approval",Model = "EqualShares",Name="Test Election",TotalBudget = 10};
        _repository.Setup(x => x.CreateAsync(newElection)).ReturnsAsync(electionToReturn);
        //Act
        var result = await _service.CreateElectionAsync(newElection);
        //Assert
        Assert.Equal(electionToReturn.Id, result?.Id);
        Assert.Equal(electionToReturn.BallotDesign, result?.BallotDesign);
        Assert.Equal(electionToReturn.Model, result?.Model);
        Assert.Equal(electionToReturn.TotalBudget, result?.TotalBudget);
        Assert.IsType<Election>(result);
    }

    
    [Fact]
    public async Task UpdateElectionAsync_UpdateElection_ReturnElection()
    {
        //Arrange
        var electionToUpdate = new Election() { Id = Guid.NewGuid() ,BallotDesign = "1-approval",Model = "EqualShares",Name="Test Election", TotalBudget = 10 };
        var electionToReturn = _mapper.Map<ElectionEntity>(electionToUpdate);
        _repository.Setup(x => x.UpdateAsync(It.IsAny<ElectionEntity>())).ReturnsAsync(electionToReturn);
        
        _repository.Setup(x => x.UpdateAsync(It.IsAny<ElectionEntity>())).ReturnsAsync(electionToReturn);
        //Act
        var result = await _service.UpdateElectionAsync(electionToUpdate);
        
        //Assert 
        Assert.NotNull(result);
        Assert.Equal(electionToReturn.Id, result.Id);
        Assert.Equal(electionToReturn.BallotDesign, result?.BallotDesign);
        Assert.Equal(electionToReturn.Model, result?.Model);
        Assert.Equal(electionToReturn.TotalBudget, result?.TotalBudget);
        Assert.IsType<Election>(result);
    }

    [Fact]
    public async Task UpdateElectionAsync_NotExistElection_ReturnNull()
    {
        //Arrange
        var electionToUpdate = new Election() {Name = "None", BallotDesign = "None", Model = "None", TotalBudget = 10 };
        var electionToArgument = _mapper.Map<ElectionEntity>(electionToUpdate);
        _repository.Setup(x=> x.UpdateAsync(It.IsAny<ElectionEntity>())).ReturnsAsync((ElectionEntity)null);
        //Act
        var result = await _service.UpdateElectionAsync(electionToUpdate);
        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteElectionAsync_DeleteElection_ReturnTrue()
    {
        //Arrange
        var electionToDelete = new Election{Id = Guid.NewGuid(),Name = "Test",TotalBudget = 0,Model = "Test",BallotDesign = "Test"};
        _repository.Setup(x => x.DeleteAsync(electionToDelete.Id)).ReturnsAsync(true);
        //Act
        var result = await _service.DeleteByIdAsync(electionToDelete.Id);
        //Assert
        Assert.True(result);
    } 
    
    
    [Fact]
    public async Task DeleteElectionAsync_DeleteElection_ReturnFalse()
    {
        //Arrange
        var electionToDelete = new Election{Id = Guid.NewGuid(),Name = "Test",TotalBudget = 0,Model = "Test",BallotDesign = "Test"};
        _repository.Setup(x => x.DeleteAsync(electionToDelete.Id)).ReturnsAsync(true);
        //Act
        var result = await _service.DeleteByIdAsync(electionToDelete.Id);
        //Assert
        Assert.True(result);
    } 
}