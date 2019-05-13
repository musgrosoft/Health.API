using System.Threading.Tasks;
using Importer.Withings.Domain;
using Moq;
using Services.OAuth;
using Utils;
using Xunit;

namespace Importer.Withings.Tests.Unit
{
    public class WithingsAuthenticatorTests
    {
        private Mock<ITokenService> _tokenService;
        private WithingsAuthenticator _withingsAuthenticator;
        private Mock<IWithingsClient> _withingsClient;
        private Mock<ILogger> _logger;

        public WithingsAuthenticatorTests()
        {
            _tokenService = new Mock<ITokenService>();
            _withingsClient = new Mock<IWithingsClient>();
            _logger = new Mock<ILogger>();

            _withingsAuthenticator = new WithingsAuthenticator(_tokenService.Object, _withingsClient.Object, _logger.Object);
        }



        [Fact]
        public async Task ShouldSetTokens()
        {
            //Given
            var authorizationCode = "sdsffewrwerewr";
            var accessToken = "sdfwert45twe";
            var refreshToken = "sdfsdf454767utgj";

            _withingsClient.Setup(x => x.GetTokensByAuthorisationCode(authorizationCode)).Returns(Task.FromResult(
                new WithingsTokenResponse
                {
                    access_token = accessToken,
                    refresh_token = refreshToken
                }));

            //When
            await _withingsAuthenticator.SetTokens(authorizationCode);

            //Then
            _tokenService.Verify(x => x.SaveWithingsAccessToken(accessToken));
            _tokenService.Verify(x => x.SaveWithingsRefreshToken(refreshToken));

        }

        [Fact]
        public async Task ShouldGetTokens()
        {
            //Given
            var refreshToken = "sdfsf4w55yujytjghdfg";
            var newTokens = new WithingsTokenResponse
            {
                access_token = "dfsdsfsfdf",
                refresh_token = "gfdghhtrjtyj"
            };

            _tokenService.Setup(x => x.GetWithingsRefreshToken()).Returns(Task.FromResult(refreshToken));
            _withingsClient.Setup(x => x.GetTokensByRefreshToken(refreshToken)).Returns(Task.FromResult(newTokens));

            //When
            var accessToken = await _withingsAuthenticator.GetAccessToken();

            //Then
            _tokenService.Verify(x => x.SaveWithingsAccessToken(newTokens.access_token));
            _tokenService.Verify(x => x.SaveWithingsRefreshToken(newTokens.refresh_token));
            Assert.Equal(newTokens.access_token, accessToken);

        }
    }
}