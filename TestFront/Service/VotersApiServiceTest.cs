using System.Net;
using DTO.Models;
using Front.Services.ApiService;
using Front.Utilities.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace TestFrontend.Services;

public class VotersApiServiceTest
{
    private readonly Mock<IHttpClientFactory> _clientFactory;
    private readonly Mock<ILogger<VotersApiService>> _logger;
    private readonly Mock<HttpMessageHandler> _handler;
    private readonly HttpClient _client;
    private readonly string _baseUrl;
    private VotersApiService _votersApiService;

    public VotersApiServiceTest()
    {
     _clientFactory = new Mock<IHttpClientFactory>();
     _logger = new Mock<ILogger<VotersApiService>>();
     _handler = new Mock<HttpMessageHandler>();
     _baseUrl = "http://mock-url.com";
     _client = new HttpClient(_handler.Object)
     {
         BaseAddress = new Uri(_baseUrl)
     };
     _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_client);
     _votersApiService = new VotersApiService(_clientFactory.Object, _logger.Object);
     
    }

    [Fact]
    private async Task GetVoterById_ValidId_returnVoter()
    {
        //Arrange
        var voter = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.OK, Content = new StringContent($@"
            {{
              ""id"": ""{voter.Id}"",
              ""electionId"": ""{voter.ElectionId}"",
              ""votes"": []
            }}
            ")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Get &&
                request.RequestUri != null &&
                request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters/{voter.Id}"),
            ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(responseMsgFromApi).Verifiable();
        
        //Act
        var result = await _votersApiService.GetVoterById(voter.Id);
        //Assert
        Assert.Equal(voter.Id, result.Id);
    }
    
    [Fact]
    private async Task GetVoterById_ValidId_InternalServerError()
    {
        //Arrange
        var voter = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent($@"
            {{
              ""id"": ""{voter.Id}"",
              ""electionId"": ""{voter.ElectionId}"",
              ""votes"": []
            }}
            ")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Get &&
                    request.RequestUri != null &&
                    request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters/{voter.Id}"),
                ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(responseMsgFromApi).Verifiable();
        
        //Act
        var result = async () => await _votersApiService.GetVoterById(voter.Id);
        //Assert
        await Assert.ThrowsAsync<InternalServerErrorException>(result);
    }  
    
    
    [Fact]
    private async Task GetVoterById_IdNotExist_NotFound()
    {
        //Arrange
        var voter = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
        var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.OK, Content = new StringContent($@"null")};
        _handler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Get &&
                    request.RequestUri != null &&
                    request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters/{voter.Id}"),
                ItExpr.IsAny<CancellationToken>()).
            ReturnsAsync(responseMsgFromApi).Verifiable();
        
        //Act
        var result = async () => await _votersApiService.GetVoterById(voter.Id);
        //Assert
        await Assert.ThrowsAsync<NotFoundError>(result);
    }       
    
    [Fact]
    private async Task GetVoterById_HandlerNotResponding_ThrowsException()
    {
        //Arrange
        var voter = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
        //Act
        var result = async () => await _votersApiService.GetVoterById(voter.Id);
        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(result);
    }


    [Fact]
    private async Task GetVotersByElectionId_ValidId_returnVoters()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK ,Content = new StringContent($@"
            [
              {{
                ""id"": ""{voter1.Id}"",
                ""electionId"": ""{voter1.ElectionId}"",
                ""votes"": []
              }},
              {{
                ""id"": ""dd2d825f-0d3c-45c7-8a25-79026cf20076"",
                ""electionId"": ""536027ad-3997-464c-9b97-2f7c84975213"",
                ""votes"": []
              }}
            ]
            ")};

       _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
           ItExpr.Is<HttpRequestMessage>(request =>
               request.Method == HttpMethod.Get &&
               request.RequestUri != null &&
               request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters" ),
           ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();
       
       //Act
       var result = await _votersApiService.GetVotersByElectionId(voter1.ElectionId);
       //Assert
       Assert.NotNull(result);
       Assert.NotEmpty(result);
       Assert.Equal(result.First().Id, voter1.Id);
    }
    
    
    
    
    [Fact]
    private async Task GetVotersByElectionId_EmptyList_returnEmptyList()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK ,Content = new StringContent($@"
            []
            ")};

       _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
           ItExpr.Is<HttpRequestMessage>(request =>
               request.Method == HttpMethod.Get &&
               request.RequestUri != null &&
               request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters" ),
           ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();
       
       //Act
       var result = await _votersApiService.GetVotersByElectionId(voter1.ElectionId);
       //Assert
       Assert.NotNull(result);
       Assert.Empty(result);
    }
       
    [Fact]
    private async Task GetVotersByElectionId_InternalServerError_ThrowsInternalServerError()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError ,Content = new StringContent($@"
            []
            ")};

       _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
           ItExpr.Is<HttpRequestMessage>(request =>
               request.Method == HttpMethod.Get &&
               request.RequestUri != null &&
               request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters" ),
           ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();
       
       //Act
       var result = async () => await _votersApiService.GetVotersByElectionId(voter1.ElectionId);
       //Assert
       await Assert.ThrowsAsync<InternalServerErrorException>(result);
    }
    
    
    [Fact]
    private async Task GetVotersByElectionId_ListIsNull_ThrowsNotFoundError()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK ,Content = new StringContent($@"
            null
            ")};

       _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
           ItExpr.Is<HttpRequestMessage>(request =>
               request.Method == HttpMethod.Get &&
               request.RequestUri != null &&
               request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters" ),
           ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();
       
       //Act
       var result = async () => await _votersApiService.GetVotersByElectionId(voter1.ElectionId);
       //Assert
       await Assert.ThrowsAsync<NotFoundError>(result);
    }
    
    
    [Fact]
    private async Task GetVotersByElectionId_HandlerNotResponding_ThrowsException()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       //Act
       var result = async () => await _votersApiService.GetVotersByElectionId(voter1.ElectionId);
       //Assert
       Assert.ThrowsAsync<InvalidOperationException>(result);
    }
    
    
    [Fact]
    private async Task CreateVoter_ValidElection()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK ,Content = new StringContent($@"
            null
            ")};

       _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
           ItExpr.Is<HttpRequestMessage>(request =>
               request.Method == HttpMethod.Post &&
               request.RequestUri != null &&
               request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters" ),
           ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();
       
       //Act
       var result = await _votersApiService.CreateVoter(voter1.ElectionId);
       //Assert
       Assert.Equal(StatusCodes.Status201Created,result);
    }
    
    
    [Fact]
    private async Task CreateVoter_ValidElection_InternalServerError5000()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError ,Content = new StringContent($@"
            null
            ")};

       _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
           ItExpr.Is<HttpRequestMessage>(request =>
               request.Method == HttpMethod.Post &&
               request.RequestUri != null &&
               request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/voters" ),
           ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response).Verifiable();
       
       //Act
       var result = async () => await _votersApiService.CreateVoter(voter1.ElectionId);
       //Assert
       await Assert.ThrowsAsync<InternalServerErrorException>(result);
    }
    
    
    [Fact]
    private async Task CreateVoter_HandlerNotResponding()
    {
       //Arrange
       var voter1 = new Voter(){Id = Guid.NewGuid(),ElectionId = Guid.NewGuid()};
       
       //Act
       var result = async () => await _votersApiService.CreateVoter(voter1.ElectionId);
       //Assert
       await Assert.ThrowsAsync<InvalidOperationException>(result);
    }
    
       
}