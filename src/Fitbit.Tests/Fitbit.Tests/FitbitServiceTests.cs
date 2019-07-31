//using System;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using Importer.Fitbit;
//using Moq;
//using Moq.Protected;
//using Services.OAuth;
//using Utils;
//using Xunit;
//
//namespace Fitbit.Tests
//{
//    public class FitbitServiceTests
//    {
//        private Mock<ITokenService> _tokenService;
//        private Mock<HttpMessageHandler> _httpMessageHandler;
//        private HttpRequestMessage _capturedRequest;
//        private HttpClient _httpClient;
//        private Mock<IConfig> _config;
//        private Mock<ILogger> _logger;
//        private FitbitService _fitbitService;
//
//        private string _content = "";
//
//        public FitbitServiceTests()
//        {
//            _tokenService = new Mock<ITokenService>();
//
//            _httpMessageHandler = new Mock<HttpMessageHandler>();
//
//            _capturedRequest = new HttpRequestMessage();
//
//            _httpClient = new HttpClient(_httpMessageHandler.Object);
//
//            _config = new Mock<IConfig>();
//
//            _logger = new Mock<ILogger>();
//
//            _fitbitService = new FitbitService(_tokenService.Object, _httpClient, _config.Object, _logger.Object);
//
//            Uri _capturedUri = new Uri("http://www.null.com");
//
//            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
//                .Returns(Task.FromResult(new HttpResponseMessage
//                {
//                    StatusCode = HttpStatusCode.OK,
//                    Content = new StringContent(_content)
//                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;
//
//            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
//                .Returns(Task.FromResult(new HttpResponseMessage
//                {
//                    StatusCode = HttpStatusCode.OK,
//                    Content = new StringContent(@"{                    
//                                                            ""access_token"": ""eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE0MzAzNDM3MzUsInNjb3BlcyI6Indwcm8gd2xvYyB3bnV0IHdzbGUgd3NldCB3aHIgd3dlaSB3YWN0IHdzb2MiLCJzdWIiOiJBQkNERUYiLCJhdWQiOiJJSktMTU4iLCJpc3MiOiJGaXRiaXQiLCJ0eXAiOiJhY2Nlc3NfdG9rZW4iLCJpYXQiOjE0MzAzNDAxMzV9.z0VHrIEzjsBnjiNMBey6wtu26yHTnSWz_qlqoEpUlpc"",
//                                                            ""expires_in"": 3600,
//                                                            ""refresh_token"": ""c643a63c072f0f05478e9d18b991db80ef6061e4f8e6c822d83fed53e5fafdd7"",
//                                                            ""token_type"": ""Bearer"",
//                                                            ""user_id"": ""26FWFL""
//                                                        }")
//                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;
//
//
//        }
//
//        [Fact]
//        public async Task ShouldSetTokens()
//        {
//            //Given
//            var authCode = "*** this is the authorisation code ***";
//            var accesssToken = "***this is the access token***";
//            var refreshToken = "***this is the refresh token***";
//            //setup hhtpClient
//            _content = $@"{{                    
//                            ""access_token"": {accesssToken} 
//                            ""expires_in"": 3600,
//                            ""refresh_token"": {refreshToken},
//                            ""token_type"": ""Bearer"",
//                            ""user_id"": ""26FWFL""
//                        }}";
//
//            //When
//            await _fitbitService.SetTokens(authCode);
//
//            //
//            _tokenService.Verify(x=>x.SaveFitbitAccessToken(accesssToken) );
//            _tokenService.Verify(x => x.SaveFitbitRefreshToken(refreshToken));
//
//
//        }
//
//        [Fact]
//        public async Task ShouldGetRestingHeartRates()
//        {
//            //Given
//            var startDate = new DateTime(2018, 1, 1);
//            var endDate = new DateTime(2019, 1, 1);
//
//            var refreshToken = "123bnmoidf89732";
//
//
//            _content = @"{                    
//                            ""access_token"": ""eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE0MzAzNDM3MzUsInNjb3BlcyI6Indwcm8gd2xvYyB3bnV0IHdzbGUgd3NldCB3aHIgd3dlaSB3YWN0IHdzb2MiLCJzdWIiOiJBQkNERUYiLCJhdWQiOiJJSktMTU4iLCJpc3MiOiJGaXRiaXQiLCJ0eXAiOiJhY2Nlc3NfdG9rZW4iLCJpYXQiOjE0MzAzNDAxMzV9.z0VHrIEzjsBnjiNMBey6wtu26yHTnSWz_qlqoEpUlpc"",
//                            ""expires_in"": 3600,
//                            ""refresh_token"": ""c643a63c072f0f05478e9d18b991db80ef6061e4f8e6c822d83fed53e5fafdd7"",
//                            ""token_type"": ""Bearer"",
//                            ""user_id"": ""26FWFL""
//                        }";
//
//
//            _tokenService.Setup(x => x.GetFitbitRefreshToken()).Returns(Task.FromResult(refreshToken));
//
//
//            //When
//            var restingHeartRates = await _fitbitService.GetRestingHeartRates(startDate, endDate);
//
//            //Then
//            Assert.Equal(1, restingHeartRates.Count());
//
//
//        }
//    }
//}