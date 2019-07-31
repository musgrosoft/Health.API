//using System.Threading.Tasks;
//using Importer.Fitbit.Domain;
//using Moq;
//using Services.OAuth;
//using Xunit;

//namespace Importer.Fitbit.Tests.Unit
//{
//    public class FitbitAuthenticatorTests
//    {
//        private Mock<ITokenService> _tokenService;
//        private FitbitAuthenticator _fitbitAuthenticator;
//        private Mock<IFitbitClient> _fitbitClient;


//        public FitbitAuthenticatorTests()
//        {
     
//            _tokenService = new Mock<ITokenService>();
//            _fitbitClient = new Mock<IFitbitClient>(); 
            
//            _fitbitAuthenticator = new FitbitAuthenticator(_tokenService.Object, _fitbitClient.Object);
//        }

//        [Fact]
//        public async Task ShouldSetTokens()
//        {
//            //Given
//            var authorisationCode = "asdasd234234dfgdfgdf";

//            _fitbitClient.Setup(x => x.GetTokensWithAuthorizationCode(authorisationCode))
//                .Returns(Task.FromResult(new FitbitAuthTokensResponse
//                {
//                    refresh_token = "bbb",
//                    access_token = "aaa"
//                }));

//            //When 
//            await _fitbitAuthenticator.SetTokens(authorisationCode);

//            //Then

//            _tokenService.Verify(x => x.SaveFitbitAccessToken("aaa"));
//            _tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb"));

//        }


//        [Fact]
//        public async Task ShouldGetAccessToken()
//        {
//            //Given
//            _tokenService.Setup(x => x.GetFitbitRefreshToken()).Returns(Task.FromResult("abc123"));

//            _fitbitClient.Setup(x => x.GetTokensWithRefreshToken("abc123"))
//                .Returns(Task.FromResult(new FitbitRefreshTokenResponse
//                {
//                    access_token = "aaa2",
//                    refresh_token = "bbb2"
//                }));


//            //When
//            await _fitbitAuthenticator.GetAccessToken();

//            //Then



//            _tokenService.Verify(x => x.SaveFitbitAccessToken("aaa2"));
//            _tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb2"));

//        }


//    }
//}