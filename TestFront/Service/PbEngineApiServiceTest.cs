using System.Net;
using DTO.Models;
using Front.Services.ApiService;
using Front.Utilities.Errors;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using MudBlazor;

namespace TestFrontend.Services;

public class PbEngineApiServiceTest
{
   private readonly Mock<IHttpClientFactory> _clientFactory;
   private readonly Mock<HttpMessageHandler> _handler;
   private readonly Mock<ILogger<PbEngineApiService>> _loggerMock;
   private readonly HttpClient _client;
   private readonly string _baseUrl;
   private PbEngineApiService _pbeService;

   public PbEngineApiServiceTest()
   {
      _clientFactory = new Mock<IHttpClientFactory>();
      _loggerMock = new Mock<ILogger<PbEngineApiService>>();
      _handler = new Mock<HttpMessageHandler>();
      _baseUrl = "https://mocking-url.com";
      _client = new HttpClient(_handler.Object)
      {
         BaseAddress = new Uri(_baseUrl)
      };
      _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_client);
      _pbeService = new PbEngineApiService(_clientFactory.Object, _loggerMock.Object);
   }

   [Fact]
   public async Task CalculateElection_validElection_ReturnListWithProject()
   {
      //Arrange
      var electionId = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213");
      var responseMsgFromApi = new HttpResponseMessage()
      {
         StatusCode = HttpStatusCode.OK,
         Content = new StringContent(@"
         [
           {
             ""id"": ""feec75d4-fe21-4c23-b5a4-61ae8bc6b30e"",
             ""electionId"": ""536027ad-3997-464c-9b97-2f7c84975213"",
             ""name"": ""p2"",
             ""cost"": 10,
             ""categories"": null,
             ""targets"": null
           }
         ]
         ")
      };
      _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
         ItExpr.Is<HttpRequestMessage>(request =>
            request.Method == HttpMethod.Get &&
            request.RequestUri != null &&
            request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/pbengine/{electionId}"
         ),
         ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi);
      //Act
      var result = await _pbeService.CalculateElection(electionId);
      //Assert
      Assert.NotNull(result);
      Assert.NotEmpty(result);
      Assert.Equal(result.First().ElectionId, electionId);
   }
   
   
   [Fact]
   public async Task CalculateElection_validElection_ReturnEmptyList()
   {
      //Arrange
      var electionId = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213");
      var responseMsgFromApi = new HttpResponseMessage()
      {
         StatusCode = HttpStatusCode.OK,
         Content = new StringContent(@"
         [
         ]
         ")
      };
      _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
         ItExpr.Is<HttpRequestMessage>(request =>
            request.Method == HttpMethod.Get &&
            request.RequestUri != null &&
            request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/pbengine/{electionId}"
         ),
         ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi);
      //Act
      var result = await _pbeService.CalculateElection(electionId);
      //Assert
      Assert.NotNull(result);
      Assert.Empty(result);
   } 
   
   
   [Fact]
   public async Task CalculateElection_getsNull_throwNotFound()
   {
      //Arrange
      var electionId = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213");
      var responseMsgFromApi = new HttpResponseMessage()
      {
         StatusCode = HttpStatusCode.OK,
         Content = new StringContent(@"
         null
         ")
      };
      _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
         ItExpr.Is<HttpRequestMessage>(request =>
            request.Method == HttpMethod.Get &&
            request.RequestUri != null &&
            request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/pbengine/{electionId}"
         ),
         ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi);
      //Act
      var result = async () => await _pbeService.CalculateElection(electionId);
      //Assert
      await Assert.ThrowsAsync<NotFoundError>(result);
   } 
   
   [Fact]
   public async Task CalculateElection_InternalServerError_throwInternalServerError()
   {
      //Arrange
      var electionId = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213");
      var responseMsgFromApi = new HttpResponseMessage()
      {
         StatusCode = HttpStatusCode.InternalServerError,
         Content = new StringContent(@"
         ")
      };
      _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
         ItExpr.Is<HttpRequestMessage>(request =>
            request.Method == HttpMethod.Get &&
            request.RequestUri != null &&
            request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/pbengine/{electionId}"
         ),
         ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi);
      //Act
      var result = async () => await _pbeService.CalculateElection(electionId);
      //Assert
      await Assert.ThrowsAsync<InternalServerErrorException>(result);
   } 
   
   
   
   [Fact]
   public async Task CalculateElection_HandlerNotResponding_ThrowException()
   {
      //Arrange
      var electionId = Guid.Parse("536027ad-3997-464c-9b97-2f7c84975213");
      //Act
      var result = async () => await _pbeService.CalculateElection(electionId);
      //Assert
      await Assert.ThrowsAsync<InvalidOperationException>(result);
   } 
   
}