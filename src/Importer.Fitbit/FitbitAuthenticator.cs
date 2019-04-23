using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Importer.Fitbit.Domain;
using Newtonsoft.Json;
using Services.OAuth;
using Utils;

namespace Importer.Fitbit
{
    public class FitbitAuthenticator : IFitbitAuthenticator
    {
        private readonly ITokenService _tokenService;
        private readonly IConfig _config;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;


        public FitbitAuthenticator(ITokenService tokenService, IConfig config, HttpClient httpClient, ILogger logger)
        {
            _tokenService = tokenService;
            _config = config;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SetTokens(string authorizationCode)
        {
            var tokens = await GetTokensWithAuthorizationCode(authorizationCode);

            var tokenResponseAccessToken = tokens.access_token;
            var tokenResponseRefreshToken = tokens.refresh_token;

            await _tokenService.SaveFitbitAccessToken(tokenResponseAccessToken);
            await _tokenService.SaveFitbitRefreshToken(tokenResponseRefreshToken);

        }



        public async Task<string> GetAccessToken()
        {
            var refreshToken = await _tokenService.GetFitbitRefreshToken();

            var newTokens = await GetTokensWithRefreshToken(refreshToken);

            var newAccessToken = newTokens.access_token;
            var newRefreshToken = newTokens.refresh_token;
            
            await _tokenService.SaveFitbitAccessToken(newAccessToken);
            await _tokenService.SaveFitbitRefreshToken(newRefreshToken);

            return newAccessToken;
        }

        private async Task<FitbitRefreshTokenResponse> GetTokensWithRefreshToken(string refreshToken)
        {
            var uri = $"{_config.FitbitBaseUrl}/oauth2/token";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + "MjI4UFI4OjAyZjIyODBkOTY2MWQwMWFiNDlkY2Q1NWJhMjE4OTFh");
            //    client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            };

            var response = await _httpClient.PostAsync(uri, new FormUrlEncodedContent(nvc));


            //            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<FitbitRefreshTokenResponse>(responseBody);
        }

        private async Task<FitbitAuthTokensResponse> GetTokensWithAuthorizationCode(string authorizationCode)
        {
            var url = $"{_config.FitbitBaseUrl}/oauth2/token";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Base64Encode($"{_config.FitbitClientId}:{_config.FitbitClientSecret}"));

            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", _config.FitbitClientId),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("redirect_uri", "http://musgrosoft-health-api.azurewebsites.net/api/fitbit/oauth/"),
            };

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await _logger.LogMessageAsync($"Fitbit SetTokens SUCCESS status code : {response.StatusCode} , content: {responseBody}");
                var tokenResponse = JsonConvert.DeserializeObject<FitbitAuthTokensResponse>(responseBody);
                //var tokenResponse = JsonConvert.DeserializeObject<NokiaTokenResponse>(responseBody);

                return tokenResponse;
            }
            else
            {
                //await _logger.LogMessageAsync($"Fitbit SetTokens FAIL  non success status code : {response.StatusCode} , content: {responseBody}");
                throw new Exception($"Fitbit SetTokens FAIL  non success status code : {response.StatusCode} , content: {responseBody}");
            }

        }


        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


    }
}
