﻿using System.Net.Http;
using System.Threading.Tasks;
using Services.OAuth;
using Utils;

namespace Fitbit
{
    public class FitbitAuthenticator : IFitbitAuthenticator
    {
        private readonly ITokenService _tokenService;
        private readonly IFitbitClient _fitbitClient;


        public FitbitAuthenticator(ITokenService tokenService, IFitbitClient fitbitClient)
        {
            _tokenService = tokenService;
            _fitbitClient = fitbitClient; 
        }

        public async Task SetTokens(string authorizationCode)
        {
            var tokens = await _fitbitClient.GetTokensWithAuthorizationCode(authorizationCode);

            var tokenResponseAccessToken = tokens.access_token;
            var tokenResponseRefreshToken = tokens.refresh_token;

            await _tokenService.SaveFitbitAccessToken(tokenResponseAccessToken);
            await _tokenService.SaveFitbitRefreshToken(tokenResponseRefreshToken);

        }



        public async Task<string> GetAccessToken()
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
