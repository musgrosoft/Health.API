using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Importer.Withings.Domain;
using Newtonsoft.Json;
using Services.OAuth;
using Utils;

namespace Importer.Withings
{
    public class WithingsAuthenticator : IWithingsAuthenticator
    {
        private readonly ITokenService _oAuthService;

        private readonly IWithingsClient _withingsClient;


        public WithingsAuthenticator(ITokenService oAuthService, IWithingsClient withingsClient)
        {
            _oAuthService = oAuthService;
            _withingsClient = withingsClient;
        }

        public async Task SetTokens(string authorizationCode)
        {
            var tokenResponse = await _withingsClient.GetTokensByAuthorisationCode(authorizationCode);

            var tokenResponseAccessToken = tokenResponse.access_token;
            var tokenResponseRefreshToken = tokenResponse.refresh_token;

            await _oAuthService.SaveWithingsAccessToken(tokenResponseAccessToken);
            await _oAuthService.SaveWithingsRefreshToken(tokenResponseRefreshToken);
        }

        public async Task<string> GetAccessToken()
        {
            var refreshToken = await _oAuthService.GetWithingsRefreshToken();

            var newTokens = await _withingsClient.GetTokensByRefreshToken(refreshToken);
            
            await _oAuthService.SaveWithingsAccessToken(newTokens.access_token);
            await _oAuthService.SaveWithingsRefreshToken(newTokens.refresh_token);
            
            return newTokens.access_token;
        }



    }
}
