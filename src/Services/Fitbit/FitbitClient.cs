﻿using Newtonsoft.Json;
using Services.Fitbit.Domain;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

        public FitbitClient(HttpClient httpClient, IConfig config, IFitbitAuthenticator fitbitAuthenticator, ILogger logger)
        {
            _httpClient = httpClient;
            _config = config;
            _fitbitAuthenticator = fitbitAuthenticator;
            _logger = logger;

        }

        [NotNull]
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
            else if (response.StatusCode == (HttpStatusCode)429)
            {
                throw new TooManyRequestsException($"Too many requests made to Fitbit API. Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
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
            else if (response.StatusCode == (HttpStatusCode)429)
            {
                throw new TooManyRequestsException($"Too many requests made to Fitbit API. Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
            }
            else
            {

                throw new Exception($"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
                //_logger.Log($"No FitbitDailyActivity found for date : {date}");
                //return null;
            }
        }

    }
}
