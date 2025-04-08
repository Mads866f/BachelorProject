using Backend.Controllers.DataControllers;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestsBackend.Controllers;

public class ScoresControllerTest
{
    private readonly Mock<IScoresService> _service;
    private readonly Mock<ILogger<ScoresController>> _logger;
    private readonly ScoresController _controller;

    public ScoresControllerTest()
    {
        _service = new Mock<IScoresService>();
        _logger = new Mock<ILogger<ScoresController>>();
        _controller = new ScoresController(_service.Object, _logger.Object);
    }

    [Fact]
    public async Task UpdateScores_valid_voter_id_valid_dictionary_Return()
    {
        //Arrange
        var score = new Scores();
        var voter = Guid.NewGuid();
        var dictonary = new Dictionary<string, int>();
        dictonary.Add(Guid.NewGuid().ToString(), 1);
        _service.Setup(x => x.GetScoresForVoterIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Scores>());
        _service.Setup(x => x.DeleteByIdAsync(voter,It.IsAny<Guid>())).ReturnsAsync(true);
        _service.Setup(x => x.CreateVotersAsync(It.IsAny<Scores>())).ReturnsAsync(score);
        //Act
        var result = await _controller.UpdateScores(voter, dictonary);
        //Assert
        Assert.IsType<OkResult>(result);

    }
    
    
    
    [Fact]
    public async Task UpdateScores_Nonvalid_voter_id_valid_dictionary_Return()
    {
        //Arrange
        var voter = Guid.NewGuid();
        var dictonary = new Dictionary<string, int>();
        dictonary.Add(Guid.NewGuid().ToString(), 1);
        var score = new Scores();
        _service.Setup(x => x.GetScoresForVoterIdAsync(It.IsAny<Guid>())).ReturnsAsync((List<Scores>)null);
        _service.Setup(x => x.DeleteByIdAsync(voter,It.IsAny<Guid>())).ReturnsAsync(true);
        _service.Setup(x => x.CreateVotersAsync(It.IsAny<Scores>())).ReturnsAsync((score));
        //Act
        var result = await _controller.UpdateScores(voter, dictonary);
        //Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateScores_DeletionFail_return_500()
    {
        //Arrange
        var voter = Guid.NewGuid();
        var dictonary = new Dictionary<string, int>();
        dictonary.Add(Guid.NewGuid().ToString(), 1);
        var score = new Scores();
        _service.Setup(x => x.GetScoresForVoterIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Scores>(){score});
        _service.Setup(x => x.DeleteByIdAsync(voter,It.IsAny<Guid>())).ReturnsAsync(false);
        _service.Setup(x => x.CreateVotersAsync(It.IsAny<Scores>())).ReturnsAsync(score);
        //Act
        var result = await _controller.UpdateScores(voter, dictonary);
        //Assert
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var statusCodeResult = Assert.IsType<ObjectResult>(actionResult); 
        Assert.Equal(500, statusCodeResult.StatusCode); 
    }
    
    
    [Fact]
    public async Task UpdateScores_CreationFail_return_500()
    {
        //Arrange
        var voter = Guid.NewGuid();
        var dictonary = new Dictionary<string, int>();
        dictonary.Add(Guid.NewGuid().ToString(), 1);
        var score = new Scores();
        _service.Setup(x => x.GetScoresForVoterIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<Scores>(){score});
        _service.Setup(x => x.DeleteByIdAsync(voter,It.IsAny<Guid>())).ReturnsAsync(true);
        _service.Setup(x => x.CreateVotersAsync(It.IsAny<Scores>())).ReturnsAsync((Scores)null);
        //Act
        var result = await _controller.UpdateScores(voter, dictonary);
        //Assert
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var statusCodeResult = Assert.IsType<ObjectResult>(actionResult); 
        Assert.Equal(500, statusCodeResult.StatusCode); 
    }
    
}