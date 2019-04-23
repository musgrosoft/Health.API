using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Services.OAuth;
using Utils;
using Xunit;

namespace Importer.Fitbit.Tests
{
    public class FitbitAuthenticatorTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private Mock<ITokenService> _tokenService;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;
        private FitbitAuthenticator _fitbitAuthenticator;
        private HttpClient _httpClient;


        public FitbitAuthenticatorTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _tokenService = new Mock<ITokenService>();
            _config = new Mock<IConfig>();

            _config.Setup(x => x.FitbitBaseUrl).Returns("https://api.fitbit.com");

            _config.Setup(x => x.FitbitClientId).Returns("123456");

            _config.Setup(x => x.FitbitClientSecret).Returns("secret");


            _logger = new Mock<ILogger>();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _fitbitAuthenticator = new FitbitAuthenticator(_tokenService.Object, _config.Object, _httpClient, _logger.Object);
        }

        [Fact]
        public async Task ShouldSetTokens()
        {
            //Given
            var authorisationCode = "asdasd234234dfgdfgdf";

            Uri _capturedUri = new Uri("http://www.null.com");
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
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h); ;



            //When 
            await _fitbitAuthenticator.SetTokens(authorisationCode);

            //Then

            Assert.Equal("https://api.fitbit.com/oauth2/token", capturedRequest.RequestUri.AbsoluteUri);
            Assert.True(capturedRequest.Headers.Contains("Authorization"));
            Assert.Equal(new List<string>{ "Basic " + Base64Encode("123456:secret" )}, capturedRequest.Headers.GetValues("Authorization"));
            //Assert headers
            //Assert posted parameters

            _tokenService.Verify(x => x.SaveFitbitAccessToken("aaa"));
            _tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb"));

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [Fact]
        public async Task ShouldGetAccessToken()
        {



        }
    }
}