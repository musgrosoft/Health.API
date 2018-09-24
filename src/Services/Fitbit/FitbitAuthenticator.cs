using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Fitbit.Domain;
using Services.OAuth;
using Utils;

namespace Services.Fitbit
{
    public class FitbitAuthenticator : IFitbitAuthenticator
    {
        private readonly ITokenService _oAuthService;
        private readonly IFitbitClient _fitbitClient;

        
        public FitbitAuthenticator(ITokenService oAuthService, IFitbitClient fitbitClient)
        {
            _oAuthService = oAuthService;
            
            _fitbitClient = fitbitClient;
        }

        public async Task SetTokens(string authorizationCode)
        {
            var tokens = await _fitbitClient.GetTokensWithAuthorizationCode(authorizationCode);

            var tokenResponseAccessToken = tokens.access_token;
            var tokenResponseRefreshToken = tokens.refresh_token;

            await _oAuthService.SaveFitbitAccessToken(tokenResponseAccessToken);
            await _oAuthService.SaveFitbitRefreshToken(tokenResponseRefreshToken);

        }



        public async Task<string> GetAccessToken()
        {
            var refreshToken = await _oAuthService.GetFitbitRefreshToken();

            var newTokens = await _fitbitClient.GetTokensWithRefreshToken(refreshToken);

            var newAccessToken = newTokens.access_token;
            var newRefreshToken = newTokens.refresh_token;
            
            await _oAuthService.SaveFitbitAccessToken(newAccessToken);
            await _oAuthService.SaveFitbitRefreshToken(newRefreshToken);

            return newAccessToken;
        }




    }
}
