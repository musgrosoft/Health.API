using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Importer.Fitbit.Domain;
using Moq;
using Moq.Protected;
using Services.OAuth;
using Xunit;

namespace Importer.Fitbit.Tests
{
    public class FitbitAuthenticatorTests
    {
        private Mock<ITokenService> _tokenService;
        private FitbitAuthenticator _fitbitAuthenticator;
        private Mock<IFitbitClient> _fitbitClient;


        public FitbitAuthenticatorTests()
        {
     
            _tokenService = new Mock<ITokenService>();
            _fitbitClient = new Mock<IFitbitClient>(); 
            
            _fitbitAuthenticator = new FitbitAuthenticator(_tokenService.Object, _fitbitClient.Object);
        }

        [Fact]
        public async Task ShouldSetTokens()
        {
            //Given
            var authorisationCode = "asdasd234234dfgdfgdf";

            _fitbitClient.Setup(x => x.GetTokensWithAuthorizationCode(authorisationCode))
                .Returns(Task.FromResult(new FitbitAuthTokensResponse
                {
                    refresh_token = "bbb",
                    access_token = "aaa"
                }));

            //When 
            await _fitbitAuthenticator.SetTokens(authorisationCode);

            //Then

            _tokenService.Verify(x => x.SaveFitbitAccessToken("aaa"));
            _tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb"));

        }


        //[Fact]
        //public async Task ShouldGetAccessToken()
        //{
        //    //Given
        //    _tokenService.Setup(x => x.GetFitbitRefreshToken()).Returns(Task.FromResult("abc123"));

        //    HttpRequestMessage capturedRequest = new HttpRequestMessage();
        //    _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        //        .Returns(Task.FromResult(new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new StringContent(@"{
        //            ""access_token"": ""aaa2"",
        //            ""expires_in"": 3600,
        //            ""refresh_token"": ""bbb2"",
        //            ""scope"": ""xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"",
        //            ""token_type"": ""Bearer"",
        //            ""user_id"": ""26FWFL""
        //        }")
        //        })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);


        ////When
        //await _fitbitAuthenticator.GetAccessToken();

        //    //Then

        //    Assert.Equal("https://api.fitbit.com/oauth2/token", capturedRequest.RequestUri.AbsoluteUri);
        //    Assert.True(capturedRequest.Headers.Contains("Authorization"));
        //    Assert.Equal(new List<string> { "Basic " + Base64Encode("123456:secret") }, capturedRequest.Headers.GetValues("Authorization"));

        //    Assert.Equal(HttpMethod.Post, capturedRequest.Method);

        //    Assert.Contains("grant_type=refresh_token", (await capturedRequest.Content.ReadAsStringAsync()));
        //    Assert.Contains("refresh_token=abc123", (await capturedRequest.Content.ReadAsStringAsync()));

        //    _tokenService.Verify(x => x.SaveFitbitAccessToken("aaa2"));
        //    _tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb2"));

        //}


    }
}