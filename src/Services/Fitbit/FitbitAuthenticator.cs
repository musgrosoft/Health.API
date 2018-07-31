using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Fitbit.Domain;
using Services.OAuth;
using Utils;

namespace Services.Fitbit
{
    public class FitbitAuthenticator : IFitbitAuthenticator
    {
        private readonly IOAuthService _oAuthService;

        private readonly HttpClient _httpClient;
        private readonly IConfig _config;
        private readonly ILogger _logger;

//        private readonly ILambdaLogger _logger;
        private const string FITBIT_SERVER = "https://api.fitbit.com";
        
        public FitbitAuthenticator(IOAuthService oAuthService, HttpClient httpClient, IConfig config, ILogger logger)
        {
            _oAuthService = oAuthService;
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task SetTokens(string authorizationCode)
        {
            var url = $"{FITBIT_SERVER}/oauth2/token";
            
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Base64Encode($"{_config.FitbitClientId}:{_config.FitbitClientSecret}"));

            //req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

            //application/x-www-form-urlencoded


            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("client_id", _config.FitbitClientId),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", "http%3A%2F%2Fmusgrosoft-health-api.azurewebsites.net%2Fapi%2Ffitbit%2Foauth%2F"),
            };

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

            //var response = await _httpClient.PostAsync(url, null);

            //            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.Log($"Fitbit SetTokens SUCCESS status code : {response.StatusCode} , content: {responseBody}");
                var tokenResponse = JsonConvert.DeserializeObject<FitbitAuthTokensResponse>(responseBody);
                //var tokenResponse = JsonConvert.DeserializeObject<NokiaTokenResponse>(responseBody);

                var tokenResponseAccessToken = tokenResponse.access_token;
                var tokenResponseRefreshToken = tokenResponse.refresh_token;

                await _oAuthService.SaveFitbitAccessToken(tokenResponseAccessToken);
                await _oAuthService.SaveFitbitRefreshToken(tokenResponseRefreshToken);
            }
            else
            {
              _logger.Log($"Fitbit SetTokens FAIL  non success status code : {response.StatusCode} , content: {responseBody}");
            }

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public async Task<string> GetAccessToken()
        {
            
                var refreshToken = await _oAuthService.GetFitbitRefreshToken();

                var newTokens = await GetTokens(refreshToken);

                var newAccessToken = newTokens.access_token;
                var newRefreshToken = newTokens.refresh_token;
            
                await _oAuthService.SaveFitbitAccessToken(newAccessToken);
                await _oAuthService.SaveFitbitRefreshToken(newRefreshToken);


            return newAccessToken;
        }

        private async Task<FitbitRefreshTokenResponse> GetTokens(string refreshToken)
        {

            
            var uri = FITBIT_SERVER + "/oauth2/token";

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


    }
}
