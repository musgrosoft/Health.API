using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.OAuth;
using Services.Withings.Domain;
using Services.Withings.Domain.OAuthDomain;
using Utils;

namespace Services.Withings.Services
{
    public class WithingsAuthenticator : IWithingsAuthenticator
    {
        private readonly ITokenService _oAuthService;

        private readonly IConfig _config;

        private readonly HttpClient _httpClient;

        private readonly ILogger _logger;

        //private const string NOKIA_BASE_URL = "https://api.health.nokia.com";
        private const string NOKIA_BASE_URL = "https://account.withings.com";
        //private const string NOKIA_BASE_URL = "https://wbsapi.withings.net";

        

        private const string NOKIA_REDIRECT_URL = "https://musgrosoft-health-api.azurewebsites.net/api/nokia/oauth/";

        public WithingsAuthenticator(ITokenService oAuthService, IConfig config, HttpClient httpClient, ILogger logger)
        {
            _oAuthService = oAuthService;
            _config = config;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SetTokens(string authorizationCode)
        {
            try
            {
                var url = $"{NOKIA_BASE_URL}/oauth2/token";
                
                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", _config.NokiaClientId),
                    new KeyValuePair<string, string>("client_secret", _config.NokiaClientSecret),
                    new KeyValuePair<string, string>("code", authorizationCode),
                    new KeyValuePair<string, string>("redirect_uri", NOKIA_REDIRECT_URL)

                    
                };

                var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonConvert.DeserializeObject<WithingsTokenResponse>(responseBody);

                    var tokenResponseAccessToken = tokenResponse.access_token;
                    var tokenResponseRefreshToken = tokenResponse.refresh_token;

                    await _oAuthService.SaveNokiaAccessToken(tokenResponseAccessToken);
                    await _oAuthService.SaveNokiaRefreshToken(tokenResponseRefreshToken);
                }
                else
                {
                    await _logger.LogMessageAsync($"non success status code : {response.StatusCode} , content: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }

        }

        public async Task<string> GetAccessToken()
        {
            var refreshToken = await _oAuthService.GetNokiaRefreshToken();

            var newTokens = await GetTokens(refreshToken);

            var newAccessToken = newTokens.AccessToken;
            var newRefreshToken = newTokens.RefreshToken;

            await _oAuthService.SaveNokiaAccessToken(newAccessToken);
            await _oAuthService.SaveNokiaRefreshToken(newRefreshToken);
            
            return newAccessToken;
        }

        private async Task<Tokens> GetTokens(string refreshToken)
        {
            var url = $"{NOKIA_BASE_URL}/oauth2/token";

            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _config.NokiaClientId),
                new KeyValuePair<string, string>("client_secret", _config.NokiaClientSecret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("redirect_uri", NOKIA_REDIRECT_URL)
            };

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

            string responseBody = await response.Content.ReadAsStringAsync();
                        
            var tokenResponse = JsonConvert.DeserializeObject<WithingsTokenResponse>(responseBody);

            var tokenResponseAccessToken = tokenResponse.access_token;
            var tokenResponseRefreshToken = tokenResponse.refresh_token;

            var tokens = new Tokens {AccessToken = tokenResponseAccessToken, RefreshToken = tokenResponseRefreshToken};

            return tokens;
        }


    }
}
