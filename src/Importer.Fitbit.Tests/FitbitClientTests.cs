using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Utils;
using Xunit;

namespace Importer.Fitbit.Tests
{
    public class FitbitClientTests
    {

        
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;
        private HttpClient _httpClient;
        private FitbitClient _fitbitClient;

       // private string _accessToken;

        public FitbitClientTests()
        {


            //_httpMessageHandler = new Mock<HttpMessageHandler>();
            //_httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .Returns(Task.FromResult(new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = new StringContent(fitbitDailyActivityContent)
            //    })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

            //_httpClient = new HttpClient(_httpMessageHandler.Object);


            _httpMessageHandler = new Mock<HttpMessageHandler>();


            _config = new Mock<IConfig>();

            _config.Setup(x => x.FitbitBaseUrl).Returns("https://api.fitbit.com");

            _config.Setup(x => x.FitbitClientId).Returns("123456");

            _config.Setup(x => x.FitbitClientSecret).Returns("secret");

            _logger = new Mock<ILogger>();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _fitbitClient = new FitbitClient(_httpClient, _config.Object, _logger.Object);
        }

        [Fact]
        public async Task ShouldGetFitbitDailyActivity()
        {
            Uri _capturedUri = new Uri("http://www.null.com");
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fitbitDailyActivityContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

         //   var fitbitDailyActivity = await _fitbitClient.GetFitbitDailyActivity(new DateTime());

            //Assert.Equal("", _capturedUri.AbsoluteUri);

            //Assert.Equal(123, activity.activities.Count);


        }


        [Fact]
        public async Task ShouldGetFitbitMonthlyActivity()
        {
            Uri _capturedUri = new Uri("http://www.null.com");

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fitbitMonthlyActivityContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;



            var fitbitActivity = await _fitbitClient.GetMonthOfFitbitActivities(new DateTime(), It.IsAny<string>());

            //Assert.Equal("", _capturedUri.AbsoluteUri);

            //Assert.Equal(123, activity.activities.Count);


        }

        [Fact]
        public async Task ShouldThrowTooManyRequestsExceptionWhenStatusCode429()
        {
            Uri _capturedUri = new Uri("http://www.null.com");

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode) 429,
                    Content = new StringContent(fitbitMonthlyActivityContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

            await Assert.ThrowsAsync<TooManyRequestsException>(() => _fitbitClient.GetMonthOfFitbitActivities(new DateTime(), It.IsAny<string>()));

//            var fitbitActivity = await _fitbitClient.GetMonthOfFitbitActivities(new DateTime());
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenStatusCodeNotSuccess()
        {
            Uri _capturedUri = new Uri("http://www.null.com");

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)400,
                    Content = new StringContent("I'm a little teapot")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

            Exception ex = await Assert.ThrowsAsync<Exception>(() => _fitbitClient.GetMonthOfFitbitActivities(new DateTime(), It.IsAny<string>()));
            Assert.Contains("status code is 400", ex.Message);
            Assert.Contains("content is I'm a little teapot", ex.Message);

        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            //Given
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("wibble")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;


            _config.Setup(x => x.FitbitUserId).Returns("USER_ID_123");

            _config.Setup(x => x.FitbitBaseUrl).Returns("https://api.fitbit.com");

            _config.Setup(x => x.FitbitClientId).Returns("123456");

            _config.Setup(x => x.FitbitClientSecret).Returns("secret");

            //When
            await _fitbitClient.Subscribe("I am a token");

            //Then

            Assert.Equal("https://api.fitbit.com/1/user/USER_ID_123/apiSubscriptions/123.json", _capturedUri.AbsoluteUri);
            Assert.Equal("I am a token", _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            //also verify bearer token


        }











        [Fact]
        public async Task ShouldSetTokens()
        {
            //Given
            var authorisationCode = "asdasd234234dfgdfgdf";

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{
                    ""access_token"": ""aaa"",
                    ""expires_in"": 3600,
                    ""refresh_token"": ""bbb"",
                    ""token_type"": ""Bearer"",
                    ""user_id"": ""26FWFL""
                }")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);



            //When 
            var response = await _fitbitClient.GetTokensWithAuthorizationCode(authorisationCode);

            //Then
            Assert.Equal("aaa", response.access_token);
            Assert.Equal("bbb", response.refresh_token);

            Assert.Equal("https://api.fitbit.com/oauth2/token", capturedRequest.RequestUri.AbsoluteUri);
            Assert.True(capturedRequest.Headers.Contains("Authorization"));
            Assert.Equal(new List<string> { "Basic " + Base64Encode("123456:secret") }, capturedRequest.Headers.GetValues("Authorization"));

            Assert.Equal(HttpMethod.Post, capturedRequest.Method);

            Assert.Contains("grant_type=authorization_code", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains("client_id=123456", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains("code=asdasd234234dfgdfgdf", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains("redirect_uri=http%3A%2F%2Fmusgrosoft-health-api.azurewebsites.net%2Fapi%2Ffitbit%2Foauth%2F", (await capturedRequest.Content.ReadAsStringAsync()));


        }


        [Fact]
        public async Task ShouldGetAccessToken()
        {
            //Given
            //_tokenService.Setup(x => x.GetFitbitRefreshToken()).Returns(Task.FromResult("abc123"));

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{
                    ""access_token"": ""aaa2"",
                    ""expires_in"": 3600,
                    ""refresh_token"": ""bbb2"",
                    ""scope"": ""xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"",
                    ""token_type"": ""Bearer"",
                    ""user_id"": ""26FWFL""
                }")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);


            //When
            var response =  await _fitbitClient.GetTokensWithRefreshToken("abc123");

            //Then

            Assert.Equal("aaa2", response.access_token);
            Assert.Equal("bbb2", response.refresh_token);

            Assert.Equal("https://api.fitbit.com/oauth2/token", capturedRequest.RequestUri.AbsoluteUri);
            Assert.True(capturedRequest.Headers.Contains("Authorization"));
            Assert.Equal(new List<string> { "Basic " + Base64Encode("123456:secret") }, capturedRequest.Headers.GetValues("Authorization"));

            Assert.Equal(HttpMethod.Post, capturedRequest.Method);

            Assert.Contains("grant_type=refresh_token", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains("refresh_token=abc123", (await capturedRequest.Content.ReadAsStringAsync()));

            //_tokenService.Verify(x => x.SaveFitbitAccessToken("aaa2"));
            //_tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb2"));

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnNonSuccessStatusCodeWhenGetTokensWithRefreshToken()
        {
            //Given
            var refreshToken = "abc123";

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)500,
                    Content = new StringContent(@"This is an error 123")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);


            //When
            Exception ex = await Assert.ThrowsAsync<Exception>(() => _fitbitClient.GetTokensWithRefreshToken(refreshToken));

            //Then

            Assert.Contains("status code is : 500", ex.Message);
            Assert.Contains("This is an error 123", ex.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnNonSuccessStatusCodeWhenGetTokensWithAuthorizationCode()
        {
            //Given
            var authorisationCode = "asdasd234234dfgdfgdf";

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)404,
                    Content = new StringContent("error abc")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);



            //When 
            var ex = await Assert.ThrowsAsync<Exception>(() => _fitbitClient.GetTokensWithAuthorizationCode(authorisationCode));

            //Then
            Assert.Contains("status code is : 404", ex.Message);
            Assert.Contains("error abc", ex.Message);
        }
































        private readonly string fitbitDailyActivityContent = "";

        private readonly string fitbitMonthlyActivityContent = "";
        private Uri _capturedUri
            ;
    }






}
