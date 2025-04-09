using System.Net;
using DTO.Models;
using Front.Services.ApiService.Elections;
using Front.Utilities.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using MudBlazor;

namespace TestFrontend.Services;

public class ProjectsApiServiceTest
{
    private readonly Mock<IHttpClientFactory> _clientFactory;
    private readonly Mock<HttpMessageHandler> _handler;
    private readonly Mock<ILogger<ProjectsApiService>> _logger;
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private ProjectsApiService _projectApiService;


    public ProjectsApiServiceTest()
    {
        _clientFactory = new Mock<IHttpClientFactory>();
        _handler = new Mock<HttpMessageHandler>();
        _baseUrl = "https://mocking-url.com";
        _logger = new Mock<ILogger<ProjectsApiService>>();
        _client = new HttpClient(_handler.Object)
        {
            BaseAddress = new Uri(_baseUrl)
        };
        _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_client);
        _projectApiService = new ProjectsApiService(_clientFactory.Object, _logger.Object);
    }

    [Fact]
    public async Task GetProjectsWithElectionId_ValidElectionId_returnsListOfProjects()
    {
        //Arrange
        var electionId = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213");
        var project1 = new Project
        { ElectionId = electionId, Name = "p1", Cost = 10 };
        var project2 = new Project
        { ElectionId = electionId, Name = "p2", Cost = 10 };
        var responseMsgFromApi = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK, Content = new StringContent(@"
                [
                  {
                    ""id"": ""e004d0a4-9e8b-4ab5-89c4-e086adc78077"",
                    ""electionId"": ""536027ad-3997-464c-9b97-2f7c84975213"",
                    ""name"": ""p1"",
                    ""cost"": 10,
                    ""categories"": null,
                    ""targets"": null
                  },
                  {
                    ""id"": ""feec75d4-fe21-4c23-b5a4-61ae8bc6b30e"",
                    ""electionId"": ""536027ad-3997-464c-9b97-2f7c84975213"",
                    ""name"": ""p2"",
                    ""cost"": 10,
                    ""categories"": null,
                    ""targets"": null
                  }
                ]
                ") };
        
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.Is<HttpRequestMessage>(r => 
                    r.Method == HttpMethod.Get && 
                    r.RequestUri != null && 
                    r.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/{electionId}"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMsgFromApi).Verifiable();
        
        //Act
        var result = await _projectApiService.GetProjectsWithElectionId(electionId);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(electionId, result.First().ElectionId);
    }

    [Fact]
    public async Task GetProjectsWithElectionId_InvalidElectionId_returnsEmptyListOfProjects()
    {
       //Arrange
       var electionId = Guid.NewGuid();
       var responseMsgFromApi = new HttpResponseMessage()
       {
           StatusCode = HttpStatusCode.OK, Content = new StringContent(@"[]") };
        
       _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", 
               ItExpr.Is<HttpRequestMessage>(r => 
                   r.Method == HttpMethod.Get && 
                   r.RequestUri != null && 
                   r.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/{electionId}"),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(responseMsgFromApi).Verifiable(); 
       //Act
       var result = await _projectApiService.GetProjectsWithElectionId(electionId);
       //Assert
       Assert.NotNull(result);
       Assert.Empty(result);
    }
   
    [Fact]
    public async Task GetProjectsWithElectionId_InternalServerError_throwsInternalServerErrorException()
    {
        //Arrange
        var electionId = Guid.NewGuid();
        var responseMsgFromApi = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent(@"[]") };
        
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.Is<HttpRequestMessage>(r => 
                    r.Method == HttpMethod.Get && 
                    r.RequestUri != null && 
                    r.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/{electionId}"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMsgFromApi).Verifiable(); 
        //Act
        var result = async () => await _projectApiService.GetProjectsWithElectionId(electionId);
        //Assert
        Assert.ThrowsAsync<InternalServerErrorException>(result);
    } 
    
    [Fact]
    public async Task GetProjectsWithElectionId_HandlerNotResponding()
    {
        //Arrange
        //Act
        var result = async () => await _projectApiService.GetProjectsWithElectionId(Guid.NewGuid());
        //Assert
        Assert.ThrowsAsync<InvalidOperationException>(result);
    }

    [Fact]
    public async Task CreateProject_ValidProject_returnsOk()
    {
            //Arrange
            var electionId = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213");
            var project1 = new CreateProjectModel()
                { ElectionId = electionId, Name = "p1", Cost = 10 };
            var responseMsgFromApi = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,};
        
            _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", 
                    ItExpr.Is<HttpRequestMessage>(r => 
                        r.Method == HttpMethod.Post && 
                        r.RequestUri != null && 
                        r.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMsgFromApi).Verifiable();
        
            //Act
            var result =  await _projectApiService.CreateProject(project1);
            //Assert
            Assert.Equal(StatusCodes.Status201Created,result);
    }

    [Fact]
    public async Task CreateProject_HandlerNotResponding_ThrowsException()
    {
        //Arrange
        //Act
        var result = async () => await _projectApiService.CreateProject(new CreateProjectModel
        {
            ElectionId = default,
            Name = "",
            Cost = 10
        });
        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(result);
    }

    [Fact]
    public async Task UpdateProject_ValidProject_returnsOk()
    {
        //Arrange
        var Project = new Project
        {
            ElectionId = default,
            Name = "Test",
            Cost = 10
        };
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.OK};
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Put &&
                request.RequestUri != null &&
                request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/" 
                ),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = await _projectApiService.UpdateProject(Project);
        //Assert
        Assert.Equal(StatusCodes.Status200OK, result);
    }
    
    
    [Fact]
    public async Task UpdateProject_ValidProject_InternalServerError_throwsInternalServerErrorException()
    {
        //Arrange
        var Project = new Project
        {
            ElectionId = default,
            Name = "Test",
            Cost = 10
        };
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent(@"[]")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Put &&
                request.RequestUri != null &&
                request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/" 
                ),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = async () => await _projectApiService.UpdateProject(Project);
        //Assert
        Assert.ThrowsAsync<InternalServerErrorException>(result);
    }
    
    [Fact]
    public async Task UpdateProject_ValidProject_BadRequest_throwsInternalServerErrorException()
    {
        //Arrange
        var Project = new Project
        {
            ElectionId = default,
            Name = "Test",
            Cost = 10
        };
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(@"[]")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Put &&
                request.RequestUri != null &&
                request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/" 
            ),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = async () => await _projectApiService.UpdateProject(Project);
        //Assert
        Assert.ThrowsAsync<InternalServerErrorException>(result);
    }

    [Fact]
    public async Task UpdateProject_HandlerNotResponding_ThrowsException()
    {
        //Arrange
        var Project = new Project
        {
            ElectionId = default,
            Name = "Test",
            Cost = 10
        };
        //Act
        var result = async () => await  _projectApiService.UpdateProject(Project);
        //Assert
        Assert.ThrowsAsync<InvalidOperationException>(result);
    }

    [Fact]
    public async Task DeleteProject_ValudProject_ReturnsStatusCode200()
    {
        //Arrange
        var project = new Project
        {
            Id = Guid.NewGuid(),
            ElectionId = Guid.NewGuid(),
            Name = "Test",
            Cost = 10
        };
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.OK, Content = new StringContent(@"[]")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Delete &&
                request.RequestUri != null &&
                request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/{project.Id}" 
            ),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = await _projectApiService.DeleteProject(project);
        //Assert
        Assert.Equal(StatusCodes.Status200OK, result);
    }
    
    
    [Fact]
    public async Task DeleteProject_InValidProject_ReturnsStatusCode200()
    {
        //Arrange
        var project = new Project
        {
            Id = Guid.Empty,
            ElectionId = Guid.NewGuid(),
            Name = "Test",
            Cost = 10
        };
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.OK, Content = new StringContent(@"[]")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Delete &&
                request.RequestUri != null &&
                request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/{project.Id}" 
            ),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = await _projectApiService.DeleteProject(project);
        //Assert
        Assert.Equal(StatusCodes.Status200OK, result);
    }
       
    
    [Fact]
    public async Task DeleteProject_ValidProject_InternalServerError_throwsInternalServerErrorException()
    {
        //Arrange
        var project = new Project
        {
            Id = Guid.Empty,
            ElectionId = Guid.NewGuid(),
            Name = "Test",
            Cost = 10
        };
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent(@"[]")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Delete &&
                request.RequestUri != null &&
                request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Project/{project.Id}" 
            ),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = async () => await _projectApiService.DeleteProject(project);
        //Assert
        await Assert.ThrowsAsync<InternalServerErrorException>(result);
    }
    
    
    [Fact]
    public async Task DeleteProject_ValidProject_HandlerNotResponding_ThrowsException()
    {
        //Arrange
        var project = new Project
        {
            Id = Guid.Empty,
            ElectionId = Guid.NewGuid(),
            Name = "Test",
            Cost = 10
        };
        //Act
        var result = async () => await _projectApiService.DeleteProject(project);
        //Assert
        Assert.ThrowsAsync<InvalidOperationException>(result);
    }
    
}