using System.Net.Http;
using System.Threading.Tasks;
using Services.OAuth;
using Utils;

namespace Importer.Fitbit.Internal
{
    internal class FitbitAuthenticator //: IFitbitAuthenticator
    {
        private readonly ITokenService _tokenService;
        private readonly FitbitClient _fitbitClient;


        internal FitbitAuthenticator(ITokenService tokenService, HttpClient httpClient, IConfig config, ILogger logger)
        {
            _tokenService = tokenService;
            _fitbitClient = new FitbitClient(httpClient, config, logger); 
        }

        internal async Task SetTokens(string authorizationCode)
        {
            var tokens = await _fitbitClient.GetTokensWithAuthorizationCode(authorizationCode);

            var tokenResponseAccessToken = tokens.access_token;
            var tokenResponseRefreshToken = tokens.refresh_token;

            await _tokenService.SaveFitbitAccessToken(tokenResponseAccessToken);
            await _tokenService.SaveFitbitRefreshToken(tokenResponseRefreshToken);

        }



        internal async Task<string> GetAccessToken()
        {
            var refreshToken = await _tokenService.GetFitbitRefreshToken();

            var newTokens = await _fitbitClient.GetTokensWithRefreshToken(refreshToken);

            var newAccessToken = newTokens.access_token;
            var newRefreshToken = newTokens.refresh_token;
            
            await _tokenService.SaveFitbitAccessToken(newAccessToken);
            await _tokenService.SaveFitbitRefreshToken(newRefreshToken);

            return newAccessToken;
        }

        


    }
}
