//using System.Net.Http;
//using System.Threading.Tasks;
//using Moq;
//using Services.Fitbit;
//using Services.Fitbit.Domain;
//using Services.OAuth;
//using Xunit;

//namespace Services.Tests.Fitbit
//{
//    public class FitbitAuthenticatorTests
//    {
//        private FitbitAuthenticator _fitbitAuthenticator;
//        private Mock<ITokenService> _tokenService;
//        private Mock<IFitbitClient> _fitbitClient;

//        public FitbitAuthenticatorTests()
//        {
//            _tokenService = new Mock<ITokenService>();
//            _fitbitClient = new Mock<IFitbitClient>();

//            _fitbitAuthenticator = new FitbitAuthenticator(_tokenService.Object, _fitbitClient.Object);
//        }

//        [Fact]
//        public async Task ShouldGetAccessTokens()
//        {
//            var refreshToken = "Gin and Tonic";
//            var accessTokens = new FitbitRefreshTokenResponse
//            {
//                access_token = "ABC",
//                refresh_token = "DEF"
//            };

//            _tokenService.Setup(x => x.GetFitbitRefreshToken()).ReturnsAsync(refreshToken);

//            _fitbitClient.Setup(x => x.GetTokensWithRefreshToken(refreshToken)).ReturnsAsync(accessTokens);

//            //When
//            var returnVal = await _fitbitAuthenticator.GetAccessToken();

//            //Then
//            Assert.Equal("ABC", returnVal);
//            _tokenService.Verify(x => x.SaveFitbitAccessToken("ABC"), Times.Once);
//            _tokenService.Verify(x => x.SaveFitbitRefreshToken("DEF"), Times.Once);
//        }
//    }
//}

//private FitbitAuthenticator _fitbitAutheticator;
//private Mock<ITokenService> _tokenService;
//private Mock<IConfig> _config;

//public FitbitAuthenticatorTests()
//{
//    _tokenService = new Mock<ITokenService>();
//    _config = new Mock<IConfig>();


//    _fitbitAutheticator = new FitbitAuthenticator(_tokenService, _config, );
//}

//[Fact]
//public async Task ShouldSetTokens()
//{
//}