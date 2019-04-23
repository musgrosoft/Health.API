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
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;



            //When 
            await _fitbitAuthenticator.SetTokens(authorisationCode);

            //Then

            Assert.Equal("https://api.fitbit.com/oauth2/token", _capturedUri.AbsoluteUri);
            //Assert headers
            //Assert posted parameters

            _tokenService.Verify(x => x.SaveFitbitAccessToken("aaa"));
            _tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb"));

        }


        [Fact]
        public async Task ShouldGetAccessToken()
        {



        }
    }
}