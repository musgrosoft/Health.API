using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Fitbit.Domain;
using Services.Nokia.Domain;
using Services.OAuth;
using Services.OAuthDomain;
using Utils;

namespace Services.Nokia
{
    public class NokiaAuthenticator : INokiaAuthenticator
    {
        private readonly IOAuthService _oAuthService;

        private readonly IConfig _config;

        private readonly HttpClient _httpClient;

        private readonly ILogger _logger;

        private const string NOKIA_BASE_URL = "http://api.health.nokia.com";

        public NokiaAuthenticator(IOAuthService oAuthService, IConfig config, HttpClient httpClient, ILogger logger)
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
                //var url = $"https://account.health.nokia.com/oauth2/token?grant_type=authorization_code&client_id={_config.NokiaClientId}&client_secret={_config.NokiaClientSecret}&code={authorizationCode}";
                var url = $"https://account.health.nokia.com/oauth2/token";

                _logger.Log($"Nokia url is {url}");

                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", _config.NokiaClientId),
                    new KeyValuePair<string, string>("client_secret", _config.NokiaClientSecret),
                    new KeyValuePair<string, string>("code", authorizationCode)
                };

                var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

               // var response = await _httpClient.PostAsync(url, null);
                
                string responseBody = await response.Content.ReadAsStringAsync();

                _logger.Log($"Nokia SetAccessToken status:{response.StatusCode} and content:{responseBody}");

                var tokenResponse = JsonConvert.DeserializeObject<NokiaTokenResponse>(responseBody);

                var tokenResponseAccessToken = tokenResponse.access_token;
                var tokenResponseRefreshToken = tokenResponse.refresh_token;

                await _oAuthService.SaveNokiaAccessToken(tokenResponseAccessToken);
                await _oAuthService.SaveNokiaRefreshToken(tokenResponseRefreshToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
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

            var url = $"https://account.health.nokia.como/auth2/token?grant_type=refresh_token&client_id={_config.NokiaClientId}&client_secret={_config.NokiaClientSecret}&refresh_token={refreshToken}";

            var response = await _httpClient.PostAsync(url, null);

            _logger.Log($"Nokia SetAccessToken status:{response.StatusCode} and content:{response.Content}");

            string responseBody = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonConvert.DeserializeObject<NokiaTokenResponse>(responseBody);

            var tokenResponseAccessToken = tokenResponse.access_token;
            var tokenResponseRefreshToken = tokenResponse.refresh_token;

            var tokens = new Tokens {AccessToken = tokenResponseAccessToken, RefreshToken = tokenResponseRefreshToken};

            return tokens;
        }


    }
}
