using Newtonsoft.Json;
using Services.OAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.Google
{
    public class GoogleAuthenticator
    {

        private readonly OAuthService _oAuthService;
        //        private readonly ILambdaLogger _logger;
        private const string GOOGLE_SERVER = "https://www.googleapis.com";
        
        public GoogleAuthenticator(OAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        public async Task<string> GetAccessToken()
        {

            var refreshToken = await _oAuthService.GetGoogleRefreshToken();

            var newTokens = await GetTokens(refreshToken);

            var newAccessToken = newTokens.access_token;
//            var newRefreshToken = newTokens.refresh_token;

            await _oAuthService.SaveFitbitAccessToken(newAccessToken);
  //          await _oAuthService.SaveFitbitRefreshToken(newRefreshToken);


            return newAccessToken;
        }

        public async Task<GoogleRefreshTokenResponse> GetTokens(string refreshToken)
        {
            var client = new HttpClient();
            var uri = GOOGLE_SERVER + "/oauth2/v4/token";
            //client.DefaultRequestHeaders.Add("Authorization", "Basic " + "MjI4UFI4OjAyZjIyODBkOTY2MWQwMWFiNDlkY2Q1NWJhMjE4OTFh");
         //   client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_secret", "SHjTFZP4qSPL7aK-5Ctb1S0Q"),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("client_id", "407408718192.apps.googleusercontent.com"),
            };

            var response = await client.PostAsync(uri, new FormUrlEncodedContent(nvc));


            //            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();


            return JsonConvert.DeserializeObject<GoogleRefreshTokenResponse>(responseBody);
        }




    }
}
