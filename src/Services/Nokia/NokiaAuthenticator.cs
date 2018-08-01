﻿using System;
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

        //"https://account.health.nokia.com/oauth2_user/authorize2?response_type=code&redirect_uri=http://musgrosoft-health-api.azurewebsites.net/api/nokia/oauth/&client_id=09d4e17f36ee237455246942602624feaad12ac51598859bc79ddbd821147942&scope=user.info,user.metrics,user.activity&state=768uyFys"
        private const string NOKIA_RECIRECT_URL = "http://musgrosoft-health-api.azurewebsites.net/api/nokia/oauth/";

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
                var url = $"https://account.health.nokia.com/oauth2/token";
                
                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", _config.NokiaClientId),
                    new KeyValuePair<string, string>("client_secret", _config.NokiaClientSecret),
                    new KeyValuePair<string, string>("code", authorizationCode),
                    new KeyValuePair<string, string>("redirect_uri", NOKIA_RECIRECT_URL)

                    
                };

                var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonConvert.DeserializeObject<NokiaTokenResponse>(responseBody);

                    var tokenResponseAccessToken = tokenResponse.access_token;
                    var tokenResponseRefreshToken = tokenResponse.refresh_token;

                    await _oAuthService.SaveNokiaAccessToken(tokenResponseAccessToken);
                    await _oAuthService.SaveNokiaRefreshToken(tokenResponseRefreshToken);
                }
                else
                {
                    _logger.Log($"non success status code : {response.StatusCode} , content: {responseBody}");
                }
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
            var url = $"https://account.health.nokia.com/oauth2/token";

            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _config.NokiaClientId),
                new KeyValuePair<string, string>("client_secret", _config.NokiaClientSecret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("redirect_uri", NOKIA_RECIRECT_URL)
            };

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

            string responseBody = await response.Content.ReadAsStringAsync();
                        
            var tokenResponse = JsonConvert.DeserializeObject<NokiaTokenResponse>(responseBody);

            var tokenResponseAccessToken = tokenResponse.access_token;
            var tokenResponseRefreshToken = tokenResponse.refresh_token;

            var tokens = new Tokens {AccessToken = tokenResponseAccessToken, RefreshToken = tokenResponseRefreshToken};

            return tokens;
        }


    }
}