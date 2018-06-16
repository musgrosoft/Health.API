using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Services.Fitbit.Domain;
using Services.OAuth;
using Utils;

namespace Services.Fitbit
{
    public class FitbitAuthenticator : IFitbitAuthenticator
    {
        private readonly IOAuthService _oAuthService;
//        private readonly ILambdaLogger _logger;
        private const string FITBIT_SERVER = "https://api.fitbit.com";
        
        public FitbitAuthenticator(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
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

        public async Task<FitbitRefreshTokenResponse> GetTokens(string refreshToken)
        {

            var client = new HttpClient();
            var uri = FITBIT_SERVER + "/oauth2/token";
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + "MjI4UFI4OjAyZjIyODBkOTY2MWQwMWFiNDlkY2Q1NWJhMjE4OTFh");
            //    client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            };

            var response = await client.PostAsync(uri, new FormUrlEncodedContent(nvc));


            //            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<FitbitRefreshTokenResponse>(responseBody);
        }


    }
}
