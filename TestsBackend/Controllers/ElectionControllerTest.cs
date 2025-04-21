using Backend.Controllers.DataControllers;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestsBackend.Controllers;

public class ElectionControllerTest
{
    private readonly Mock<IElectionService> _electionService;
    private readonly Mock<ILogger<ElectionController>> _logger;
    private readonly ElectionController _controller;

    public ElectionControllerTest()
    {
        _electionService = new Mock<IElectionService>();
        _logger = new Mock<ILogger<ElectionController>>();
        _controller = new ElectionController(_electionService.Object,_logger.Object);
    }

    [Fact]
    public async Task GetAll_No_Parameters_Returns_Ok()
    {
        //Arrange
        var election1 = new Election{Name = "Test",TotalBudget = 10,Model = "EqualShares",BallotDesign = "1-Approval"};
        var election2 = new Election{Name = "Test2",TotalBudget = 10,Model = "EqualShares",BallotDesign = "1-Approval"};
        var elections = new List<Election>(){election1, election2};
        _electionService.Setup(x => x.GetAllElectionsAsync()).Returns(Task.FromResult<IEnumerable<Election>>(elections));
        //Act
        var  result = await _controller.GetElections(); 
        //Assert
        Assert.NotNull(result);
        if (result?.Value != null) Assert.Equal(result?.Value.Count(), elections.Count());
    }

    [Fact]
    public async Task GetAll_NoParameters_Returns_NotFound()
    {
        //Arrange
        var elections = new List<Election>();
        _electionService.Setup(x => x.GetAllElectionsAsync()).Returns(Task.FromResult<IEnumerable<Election>>(elections));
        //Act
        var result = await _controller.GetElections();
        //Assert
        Assert.NotNull(result);
        if (result.Value != null) Assert.Empty(result.Value);
    }

    [Fact]
    public async Task GetById_ExistId_Returns_Ok()
    {
        //Arrange
        var election = new Election{ Id = Guid.NewGuid(), Name = "Test", TotalBudget = 10, Model = "EqualShares", BallotDesign = "1-Approval"};
        _electionService.Setup(x => x.GetElectionAsync(election.Id)).ReturnsAsync(election);
        //Act
        var result = await _controller.GetById(election.Id);
        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
        if (result?.Value != null) Assert.Equal(result?.Value, election);
        
    }

    [Fact]
    public async Task GetById_ExistId_Returns_NotFound()
    {
        //Arrange
        _electionService.Setup(x => x.GetElectionAsync(It.IsAny<Guid>())).ReturnsAsync((Election)null);
        //Act
        var result = await _controller.GetById(new Guid());
        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateElection_newElection_Returns_Created()
    {
        //Arrange
        var electionToCreate = new CreateElectionModel
            { Name = "Test1", TotalBudget = 10, Model = "EqualShares", BallotDesign = "1-Approval" };
        var createdElection = new Election() 
            { Id = Guid.NewGuid(), Name = "Test1", TotalBudget = 10, Model = "EqualShares", BallotDesign = "1-Approval" };
        _electionService.Setup(x => x.CreateElectionAsync(It.IsAny<CreateElectionModel>())).ReturnsAsync(createdElection);
        
        //Act
        var result =  await _controller.CreateElection(electionToCreate);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(createdElection.Name, result.Value.Name);
    }

    [Fact]
    public async Task DeleteByIdAsync_ExistId_Returns_Ok()
    {
        //Arrange
        _electionService.Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);
        //Act
        var result = await _controller.DeleteByIdAsync(new Guid());
        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async Task DeleteByIdAsync_NOTExistId_Returns_NotFound()
    {
        //Arrange
        _electionService.Setup(x => x.DeleteByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);
        //Act
        var result = await _controller.DeleteByIdAsync(new Guid());
        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateElectionAsync_ExistId_Returns_Ok()
    {
        //Arrange
        var electionToUpdate = new Election
        { Name = "Test1", TotalBudget = 10, Model = "EqualShares", BallotDesign = "1-Approval" };
        _electionService.Setup(x => x.UpdateElectionAsync(It.IsAny<Election>())).ReturnsAsync(electionToUpdate);
        //Act
        var result = await _controller.UpdateElection(electionToUpdate);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(result.Value?.Name, electionToUpdate.Name);
    }
    
    [Fact]
    public async Task UpdateElectionAsync_NotExistId_Returns_NotFound()
    {
        //Arrange
        var electionToUpdate = new Election
            { Name = "Test1", TotalBudget = 10, Model = "EqualShares", BallotDesign = "1-Approval" };
        _electionService.Setup(x => x.UpdateElectionAsync(It.IsAny<Election>())).ReturnsAsync((Election)null);
        //Act
        var result = await _controller.UpdateElection(electionToUpdate);
        //Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
}