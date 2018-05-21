using Newtonsoft.Json;
using Services.Fitbit.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Utils;

namespace Services.Fitbit
{
    public class FitbitClient
    {
        private const string FITBIT_BASE_URL = "https://api.fitbit.com";

        private readonly HttpClient _httpClient;
        private readonly Config _config;
        private readonly string _accessToken;
        private readonly ILogger _logger;

        public FitbitClient(HttpClient httpClient, Config config, string accessToken, ILogger logger)
        {
            _httpClient = httpClient;
            _config = config;
            _accessToken = accessToken;
            _logger = logger;
        }

        
        public async Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate)
        {
            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/heart/date/{startDate:yyyy-MM-dd}/1m.json";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<FitBitActivity>(content);
                return data;
            }
            else
            {
                throw new Exception($"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
//                _logger.Log($"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
//                return Maybe<FitBitActivity>.None;
            }

        }

        public async Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date)
        {
            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/date/{date:yyyy-MM-dd}.json";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<FitbitDailyActivity>(content);

                return data;
            }
            else
            {
                return null;
            }
        }

    }
}
