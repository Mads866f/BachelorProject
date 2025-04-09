using System.Net;
using System.Runtime.CompilerServices;
using Front.Services.ApiService;
using Front.Utilities.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace TestFrontend.Services;

public class ScoresApiServiceTest
{
   private readonly Mock<IHttpClientFactory> _clientFactory;
   private readonly Mock<ILogger<ScoresApiService>> _logger;
   private readonly Mock<HttpMessageHandler> _handler;
   private readonly HttpClient _client;
   private readonly string _baseUrl;
   private readonly ScoresApiService _scoresApiService;

   public ScoresApiServiceTest()
   {
      _clientFactory = new Mock<IHttpClientFactory>();
      _logger = new Mock<ILogger<ScoresApiService>>();
      _handler = new Mock<HttpMessageHandler>();
      _baseUrl = "https://mocking-url.com";
      _client = new HttpClient(_handler.Object)
      {
         BaseAddress = new Uri(_baseUrl)
      };
      _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_client);
      _scoresApiService = new ScoresApiService(_clientFactory.Object,_logger.Object); 
   }

   [Fact]
   public async void UpdateScores_validVoterIdAndDictonary_ReturnsStatusCode200()
   {
      //Arrange
     var voterId = Guid.NewGuid();
     var votes = new Dictionary<string, int>();
     var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.OK, Content = new StringContent("")};
     _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", 
        ItExpr.Is<HttpRequestMessage>(request =>
           request.Method == HttpMethod.Post &&
           request.RequestUri != null &&
           request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/scores/{voterId}"),
        ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi);
     //Act
     var result = await _scoresApiService.UpdateScores(voterId, votes);
     //Assert
     Assert.Equal(StatusCodes.Status200OK,result);
   }
   
   [Fact]
   public async void UpdateScores_validVoterIdAndDictonary_InternalServerError_ReturnsStatusCode500()
   {
      //Arrange
      var voterId = Guid.NewGuid();
      var votes = new Dictionary<string, int>();
      var responseMsgFromApi = new HttpResponseMessage(){StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent("")};
      _handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", 
         ItExpr.Is<HttpRequestMessage>(request =>
            request.Method == HttpMethod.Post &&
            request.RequestUri != null &&
            request.RequestUri.AbsoluteUri == $"{_baseUrl}/api/scores/{voterId}"),
         ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMsgFromApi);
      //Act
      var result = async () => await _scoresApiService.UpdateScores(voterId, votes);
      //Assert
      await Assert.ThrowsAsync<InternalServerErrorException>(result);
   }
   
   
   [Fact]
   public async void UpdateScores_handlerNotResponding_ThrowsException()
   {
      //Arrange
      var voterId = Guid.NewGuid();
      var votes = new Dictionary<string, int>();
      //Act
      var result = async () => await _scoresApiService.UpdateScores(voterId, votes);
      //Assert
      await Assert.ThrowsAsync<InvalidOperationException>(result);
   }
   
   
   //TODO NEEDS TO ADD TESTS FOR INVALID VOTERS
}