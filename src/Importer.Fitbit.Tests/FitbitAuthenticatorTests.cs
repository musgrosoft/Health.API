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
        [Fact]
        public async Task ShouldSetTokens()
        {
            //Given
            var authorisationCode = "asdasd234234dfgdfgdf";

            var _httpMessageHandler = new Mock<HttpMessageHandler>();

            var tokenService = new Mock<ITokenService>();
            var config = new Mock<IConfig>();

            config.Setup(x => x.FitbitBaseUrl).Returns("https://api.fitbit.com");

            var logger = new Mock<ILogger>();
            var httpClient = new HttpClient(_httpMessageHandler.Object);

            var fitbitAuthenticator = new FitbitAuthenticator(tokenService.Object,config.Object,httpClient,logger.Object);


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
            await fitbitAuthenticator.SetTokens(authorisationCode);

            //Then

            Assert.Equal("https://api.fitbit.com/oauth2/token", _capturedUri.AbsoluteUri);
            //Assert headers
            //Assert posted parameters

            tokenService.Verify(x => x.SaveFitbitAccessToken("aaa"));
            tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb"));

        }
        
    }
}