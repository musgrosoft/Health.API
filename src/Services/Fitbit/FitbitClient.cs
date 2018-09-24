using Newtonsoft.Json;
using Services.Fitbit.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Utils;

namespace Services.Fitbit
{
    public class FitbitClient : IFitbitClient
    {
        private const string FITBIT_BASE_URL = "https://api.fitbit.com";

        private readonly HttpClient _httpClient;
        private readonly IConfig _config;

        private readonly IFitbitAuthenticator _fitbitAuthenticator;

        //private readonly string _accessToken;
        private readonly ILogger _logger;

        public FitbitClient(HttpClient httpClient, IConfig config, IFitbitAuthenticator fitbitAuthenticator,
            ILogger logger)
        {
            _httpClient = httpClient;
            _config = config;
            _fitbitAuthenticator = fitbitAuthenticator;
            _logger = logger;

        }

        //POST https://api.fitbit.com/1/user/-/apiSubscriptions/123.json

        public async Task Subscribe()
        {

            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/apiSubscriptions/123.json";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = await _httpClient.PostAsync(uri,null);

            await _logger.LogMessageAsync("Status code ::: " + response.StatusCode);
            await _logger.LogMessageAsync("content ::: " + response.Content);

        }



        public async Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate)
        {


            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/heart/date/{startDate:yyyy-MM-dd}/1m.json";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<FitBitActivity>(content);
                return data;
            }
            else if (response.StatusCode == (HttpStatusCode) 429)
            {
                throw new TooManyRequestsException(
                    $"Too many requests made to Fitbit API. Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
            }
            else
            {
                throw new Exception(
                    $"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
//                _logger.Log($"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
//                return Maybe<FitBitActivity>.None;
            }

        }

        public async Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date)
        {
            var accessToken = await _fitbitAuthenticator.GetAccessToken();


            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/date/{date:yyyy-MM-dd}.json";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<FitbitDailyActivity>(content);
                data.DateTime = date;
                return data;
            }
            else if (response.StatusCode == (HttpStatusCode) 429)
            {
                throw new TooManyRequestsException(
                    $"Too many requests made to Fitbit API. Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
            }
            else
            {

                throw new Exception(
                    $"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
                //_logger.Log($"No FitbitDailyActivity found for date : {date}");
                //return null;
            }
        }

        public async Task<FitbitRefreshTokenResponse> GetTokensWithRefreshToken(string refreshToken)
        {


            var uri = FITBIT_BASE_URL + "/oauth2/token";

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

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public async Task<FitbitAuthTokensResponse> GetTokensWithAuthorizationCode(string authorizationCode)
        {
            var url = $"{FITBIT_BASE_URL}/oauth2/token";

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

        //public async Task<List<Dataset>> GetDetailedHeartRates(DateTime date)
        //{
        //    var accessToken = await _fitbitAuthenticator.GetAccessToken();

        //    var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/heart/date/{date:yyyy-MM-dd}/1d/1sec.json";

        //    _httpClient.DefaultRequestHeaders.Clear();
        //    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

        //    var response = await _httpClient.GetAsync(uri);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        var data = JsonConvert.DeserializeObject<RootObject>(content);
        //        foreach (var dataset in data.ActivitiesHeartIntraday.dataset)
        //        {
        //            dataset.time = new DateTime(date.Year, date.Month, date.Day, dataset.time.Hour, dataset.time.Minute, dataset.time.Second);
        //        }

        //        //data.DateTime = date;
        //        return data.ActivitiesHeartIntraday.dataset;
        //    }
        //    else if (response.StatusCode == (HttpStatusCode)429)
        //    {
        //        throw new TooManyRequestsException(
        //            $"Too many requests made to Fitbit API. Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
        //    }
        //    else
        //    {

        //        throw new Exception(
        //            $"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
        //        //_logger.Log($"No FitbitDailyActivity found for date : {date}");
        //        //return null;
        //    }

        //}

    }
}
