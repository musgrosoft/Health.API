using System.Threading.Tasks;
using Services.OAuth;
using Utils;

namespace Importer.Withings
{
    public class WithingsAuthenticator : IWithingsAuthenticator
    {
        private readonly ITokenService _oAuthService;

        private readonly IWithingsClient _withingsClient;
        private readonly ILogger _logger;


        public WithingsAuthenticator(ITokenService oAuthService, IWithingsClient withingsClient, ILogger logger)
        {
            _oAuthService = oAuthService;
            _withingsClient = withingsClient;
            _logger = logger;
        }

        public async Task SetTokens(string authorizationCode)
        {
            await _logger.LogMessageAsync("~~~ Getting Withings Tokens using authorisation code : " + authorizationCode);

            var tokenResponse = await _withingsClient.GetTokensByAuthorisationCode(authorizationCode);

            await _logger.LogMessageAsync("~~~ the response is at : " + tokenResponse.access_token + " and rt : " + tokenResponse.refresh_token);

            var tokenResponseAccessToken = tokenResponse.access_token;
            var tokenResponseRefreshToken = tokenResponse.refresh_token;

            await _oAuthService.SaveWithingsAccessToken(tokenResponseAccessToken);
            await _oAuthService.SaveWithingsRefreshToken(tokenResponseRefreshToken);
        }

        public async Task<string> GetAccessToken()
        {
            var refreshToken = await _oAuthService.GetWithingsRefreshToken();

            await _logger.LogMessageAsync("~~~ Getting Withings Tokens using refresh token: " + refreshToken);

            var newTokens = await _withingsClient.GetTokensByRefreshToken(refreshToken);

            await _logger.LogMessageAsync("~~~ the response is at : " + newTokens.access_token + " and rt : " + newTokens.refresh_token);

            await _oAuthService.SaveWithingsAccessToken(newTokens.access_token);
            await _oAuthService.SaveWithingsRefreshToken(newTokens.refresh_token);
            
            return newTokens.access_token;
        }



    }
}
