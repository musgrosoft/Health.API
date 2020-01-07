using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Services.OAuth;
using Utils;
using Xunit;

namespace Fitbit
{
    public class FitbitServiceTests2
    {
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;
        private HttpRequestMessage _capturedRequest;
        private readonly HttpClient _httpClient;
        private readonly Mock<IConfig> _config;
        private readonly Mock<ILogger> _logger;
        
        private readonly FitbitService _fitbitService;
        
        public FitbitServiceTests2()
        {
            _config = new Mock<IConfig>();
            _config.Setup(x => x.FitbitBaseUrl).Returns("http://www.null.com");

            _tokenService = new Mock<ITokenService>();

            _httpMessageHandler = new Mock<HttpMessageHandler>();

            //_httpMessageHandler = new Mock<HttpMessageHandler> { CallBase = true };

            _capturedRequest = new HttpRequestMessage();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _logger = new Mock<ILogger>();

            SetupHttpMessageHandlerMock("");


            var fitbitClientQueryAdapter = new Mock<IFitbitClientQueryAdapter>();
            var fitbitAuthenticator = new Mock<IFitbitAuthenticator>();
            var fitbitMapper = new Mock<IFitbitMapper>();

            _fitbitService = new FitbitService(fitbitClientQueryAdapter.Object, fitbitAuthenticator.Object, fitbitMapper.Object);

        }

        private void SetupHttpMessageHandlerMock(string content)
        {
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);
        }



















        [Fact]
        public async Task ShouldSendCorrectHttpRequestMessage()
        {

            //Given
            
            var authCode = "*** this is the authorisation code ***";
            var accesssToken = "***this is the access token***";
            var refreshToken = "***this is the refresh token***";
            //setup hhtpClient
            var responseContent = $@"{{                    
                            ""access_token"": ""{accesssToken}"",
                            ""expires_in"": 3600,
                            ""refresh_token"": ""{refreshToken}"",
                            ""token_type"": ""Bearer"",
                            ""user_id"": ""26FWFL""
                        }}";


            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);

            //When
            await _fitbitService.SetTokens(authCode);

            var content = await capturedRequest.Content.ReadAsStringAsync();
            var requestParams = content.Split('&');

            Assert.Equal(capturedRequest.Method,HttpMethod.Post);
            Assert.Equal("http://www.null.com/oauth2/token", capturedRequest.RequestUri.AbsoluteUri);

            Assert.Contains("grant_type=authorization_code", requestParams);
        }

        [Fact]
        public async Task ShouldSetTokens()
        {
            //Given
            var capturedUri = new Uri("https://www.nulll.com");


            var authCode = "*** this is the authorisation code ***";
            var accesssToken = "***this is the access token***";
            var refreshToken = "***this is the refresh token***";
            //setup hhtpClient
            var responseContent = $@"{{                    
                            ""access_token"": ""{accesssToken}"",
                            ""expires_in"": 3600,
                            ""refresh_token"": ""{refreshToken}"",
                            ""token_type"": ""Bearer"",
                            ""user_id"": ""26FWFL""
                        }}";




            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedUri = h.RequestUri);


            //When
            await _fitbitService.SetTokens(authCode);

            //
            _tokenService.Verify(x => x.SaveFitbitAccessToken(accesssToken));
            _tokenService.Verify(x => x.SaveFitbitRefreshToken(refreshToken));


        }


//        [Fact]
//        public async Task ShouldSaveTokensToTokenServiceWhenCallingSetTokens()
//        {
//            //Given
////            var authCode = "*** this is the authorisation code ***";
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
//            await _fitbitService.SetTokens("any old auth code");
//
//            //
//            _tokenService.Verify(x => x.SaveFitbitAccessToken(accesssToken));
//            _tokenService.Verify(x => x.SaveFitbitRefreshToken(refreshToken));
//        }


        [Fact]
        public async Task ShouldMakeCorrectCallToFitbitAPIWhenCallingSetTokens()
        {
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
//            _tokenService.Verify(x => x.SaveFitbitAccessToken(accesssToken));
//            _tokenService.Verify(x => x.SaveFitbitRefreshToken(refreshToken));
        }





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
    }
}