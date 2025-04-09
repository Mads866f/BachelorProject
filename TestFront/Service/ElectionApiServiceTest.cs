using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DTO.Models;
using Front.Services.ApiService.Elections;
using Front.Utilities.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NuGet.Frameworks;
using Xunit;

public class ElectionApiServiceTest
{
    private readonly Mock<IHttpClientFactory> _clientFactory;
    private readonly Mock<HttpMessageHandler> _handler;
    private readonly Mock<ILogger<ElectionsApiService>> _loggerMock;
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private ElectionsApiService _electionsApiService;

    public ElectionApiServiceTest()
    {
        _clientFactory = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<ElectionsApiService>>();
        _handler = new Mock<HttpMessageHandler>();
        _baseUrl = "https://mocking-url.com";
        _client = new HttpClient(_handler.Object)
        {
            BaseAddress = new Uri(_baseUrl)
        };
        _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_client);
        _electionsApiService = new ElectionsApiService(_clientFactory.Object,_loggerMock.Object);
    }

    [Fact]
    public async Task GetElections_NoParam_ReturnsElections()
    {
        // Arrange
        var responseMsgFromApi = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(@"[
                    {
                      ""id"": ""536027ad-3997-464c-9b97-2f7c84975213"",
                      ""name"": ""Test"",
                      ""totalBudget"": 10,
                      ""model"": ""string"",
                      ""ballotDesign"": ""string""
                    }
                ]")
        };
        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Get &&
                    request.RequestUri != null &&
                    request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Election"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMsgFromApi).Verifiable();

        // Act
        var result = await _electionsApiService.GetElections();

        // Assert
        Assert.NotNull(result); 
        Assert.Equal("Test", result.First().Name);
    }

    [Fact]
    public async Task GetElections_NoParam_InternalServerError500()
    {
        //Arrange 
        var responseMsgFromApi= new HttpResponseMessage(){StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("")};
        
        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Get &&
                    request.RequestUri != null &&
                    request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/Election"),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMsgFromApi).Verifiable();
        
        //Act
        var result = async () =>  await _electionsApiService.GetElections();
        
        //Assert
        await Assert.ThrowsAsync<InternalServerErrorException>(result);
    }

    [Fact]
    public async Task GetElections_NoParam_HandlerNotResponding()
    {
        //Arrange
        //Act
        var result = async () => await _electionsApiService.GetElections();
        //Assert
        Assert.ThrowsAsync<InvalidOperationException>(result);
        
    }


    [Fact]
    public async Task GetElection_ValidId_ReturnsElection()
    {
        //Arrange
        var election = new Election
        { Id = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213"), Name = "Test", TotalBudget = 10, Model = "string", BallotDesign = "string" };
        var responseMsgFromApi = new HttpResponseMessage()
            { StatusCode = HttpStatusCode.OK, Content = new StringContent(@"
                {
                    ""id"": ""536027ad-3997-464c-9b97-2f7c84975213"",
                    ""name"": ""Test"",
                    ""totalBudget"": 10,
                    ""model"": ""string"",
                    ""ballotDesign"": ""string""
                }
                ") };
        
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request => 
                request.Method == HttpMethod.Get && 
                request.RequestUri != null && 
                request.RequestUri.AbsoluteUri ==  $"{_baseUrl}/api/Election/{election.Id}"),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = await _electionsApiService.GetElection(election.Id);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(election.Name, result.Name);
    }
    
    [Fact]
    public async Task GetElection_StatusCode_500_ReturnsInternalServerError()
    {
        //Arrange
        var idRequested = Guid.NewGuid();
        var responseMsgFromApi = new HttpResponseMessage()
            { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("") };
        
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri != null && request.RequestUri.AbsoluteUri ==  $"{_baseUrl}/api/Election/{idRequested}"),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = async () => await _electionsApiService.GetElection(idRequested);
        //Assert
        await Assert.ThrowsAsync<InternalServerErrorException>(result);
    }
    
    
    [Fact]
    public async Task GetElection_InvalidId_ThrowsNotFoundError()
    {
        //Arrange
        var idRequested = Guid.NewGuid();
        var responseMsgFromApi = new HttpResponseMessage()
            { StatusCode = HttpStatusCode.OK, Content = new StringContent("null") };
        
        _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri != null && request.RequestUri.AbsoluteUri ==  $"{_baseUrl}/api/Election/{idRequested}"),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var result = async () => await _electionsApiService.GetElection(idRequested);
        //Assert
        await Assert.ThrowsAsync<NotFoundError>(result);
    }
    
    [Fact]
    public async Task GetElection_SomeId_HandlerNotResponding()
    {
        //Arrange
        //Act
        var result = async () => await _electionsApiService.GetElection(Guid.NewGuid());
        //Assert
        Assert.ThrowsAsync<InvalidOperationException>(result);
    }

    [Fact]
    public async Task CreateElection_ValidElection_ReturnsElection()
    {
        //Arrange
        var election = new Election()
        { Id=Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213"), Name = "Test", TotalBudget = 10, Model = "string", BallotDesign = "string" };
        var responseMsgFromApi = new HttpResponseMessage()
            {StatusCode = HttpStatusCode.OK, Content = new StringContent(@"
                {
                    ""id"": ""536027ad-3997-464c-9b97-2f7c84975213"",
                    ""name"": ""Test"",
                    ""totalBudget"": 10,
                    ""model"": ""string"",
                    ""ballotDesign"": ""string""
                }")
                };
        _handler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Post && request.RequestUri != null && request.RequestUri.AbsoluteUri ==  $"{_baseUrl}/api/Election"),
            ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var  result = await _electionsApiService.CreateElection(election);
        //Assert
        Assert.NotNull(result);
        Assert.Equal(election.Name, result.Name);
    }
    
    
    [Fact]
    public async Task CreateElection_ValidElection_ThrowsCreationException()
    {
        //Arrange
        var election = new Election()
        { Id=Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213"), Name = "Test", TotalBudget = 10, Model = "string", BallotDesign = "string" };
        var responseMsgFromApi = new HttpResponseMessage()
            {StatusCode = HttpStatusCode.OK, Content = new StringContent(@"null")
                };
        _handler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Post && request.RequestUri != null && request.RequestUri.AbsoluteUri ==  $"{_baseUrl}/api/Election"),
            ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var  result = async () => await _electionsApiService.CreateElection(election);
        //Assert
        await Assert.ThrowsAsync<CreationException>(result);
    }
    
    [Fact]
    public async Task CreateElection_ValidElection_ThrowsInternalServerError()
    {
        //Arrange
        var election = new Election()
            { Id=Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213"), Name = "Test", TotalBudget = 10, Model = "string", BallotDesign = "string" };
        var responseMsgFromApi = new HttpResponseMessage()
        {StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent(@""), 
        };
        _handler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Post && request.RequestUri != null && request.RequestUri.AbsoluteUri ==  $"{_baseUrl}/api/Election"),
            ItExpr.IsAny<CancellationToken>()
        ).ReturnsAsync(responseMsgFromApi).Verifiable();
        //Act
        var  result = async () => await _electionsApiService.CreateElection(election);
        //Assert
        await Assert.ThrowsAsync<InternalServerErrorException>(result);
    }

    [Fact]
    public async Task GetElection_ValidElection_NoHandlerResponse()
    {
        //Arrange
        var election = new Election()
            { Id=Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213"), Name = "Test", TotalBudget = 10, Model = "string", BallotDesign = "string" };
        //Act
        var  result = async () => await _electionsApiService.CreateElection(election);
        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(result);
    }
    
    //NOTE UPDATE AND DELETE IS NOT USED OR IMPLEMENTED; THEREFOR NOT TESTED
    
}

