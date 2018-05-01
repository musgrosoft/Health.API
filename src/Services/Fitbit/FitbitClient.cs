using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Services.Fitbit.Domain;
using System.Threading.Tasks;
using Repositories.Models;
using Utils;


namespace Services.Fitbit
{
    public class FitbitClient
    {
        
        private readonly ILogger _logger;
        private readonly string _accessToken;
        private const string FITBIT_BASE_URL = "https://api.fitbit.com";
//        private static string theAccessToken;
        private const int FITBIT_HOURLY_RATE_LIMIT = 150;

        private IConfig _config { get; }

        public FitbitClient(IConfig config, ILogger logger, string accessToken)
        {
        
            _logger = logger;
            _accessToken = accessToken;
            _config = config;
        }

        

        

        public async Task<StepCount> GetDailySteps(DateTime date)
        {
            var fitbitDailyActivity = await GetActivity(date);

            if (fitbitDailyActivity != null)
            {
                return  new StepCount
                {
                    DateTime = date,
                    Count = fitbitDailyActivity.summary.steps
                };
            }

            return null;
        }

        public async Task<DailyActivity> GetDailyActivity(DateTime date)
        {
            var fitbitDailyActivity = await GetActivity(date);

            if (fitbitDailyActivity != null)
            {
                return new DailyActivity
                {
                    DateTime = date,
                    //activityCalories
                    //caloriesBMR
                    //caloriesOut
                    //distances
                    //elevation
                    FairlyActiveMinutes = fitbitDailyActivity.summary.fairlyActiveMinutes,
                    //floors
                    LightlyActiveMinutes = fitbitDailyActivity.summary.lightlyActiveMinutes,
                    //marginalCalories
                    SedentaryMinutes = fitbitDailyActivity.summary.sedentaryMinutes,
                    VeryActiveMinutes = fitbitDailyActivity.summary.veryActiveMinutes
                };
            }
            else
            {
                return null;
            }
        }

        private async Task<FitbitDailyActivity> GetActivity(DateTime date)
        {
            var client = new HttpClient();
            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/date/{date:yyyy-MM-dd}.json";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            var response = await client.GetAsync(uri);
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

        public async Task<IEnumerable<RestingHeartRate>> GetMonthOfRestingHeartRates(DateTime dateTime)
        {
            var heartSummaries = await GetMonthOfHeartSummaries(dateTime);
            //might need to check if bpm is zero
            var restingHeartRates = heartSummaries.activitiesHeart.Where(a=>a.value.restingHeartRate != 0).Select(x => new RestingHeartRate
            {
                DateTime = x.dateTime,
                Beats = x.value.restingHeartRate
            });

            return restingHeartRates;
        }

        public async Task<IEnumerable<HeartRateZoneSummary>> GetMonthOfHeartZones(DateTime date)
        {

            var heartSummaries = await GetMonthOfHeartSummaries(date);


            var smallHeartRateSummaries = heartSummaries.activitiesHeart.Select(x => new HeartRateZoneSummary
            {
                DateTime = x.dateTime,
                RestingHeartRate = x.value.restingHeartRate,
                OutOfRangeMinutes = x.value.heartRateZones.First(y => y.name == "Out of Range").minutes,
                FatBurnMinutes = x.value.heartRateZones.First(y => y.name == "Fat Burn").minutes,
                CardioMinutes = x.value.heartRateZones.First(y => y.name == "Cardio").minutes,
                PeakMinutes = x.value.heartRateZones.First(y => y.name == "Peak").minutes
            });
            

            return smallHeartRateSummaries;
        }

        private async Task<FitBitActivity> GetMonthOfHeartSummaries(DateTime startDate)
        {
            var client = new HttpClient();
            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/heart/date/{startDate:yyyy-MM-dd}/1m.json";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<FitBitActivity>(content);
                return data;
            }
            else
            {
                throw new Exception($"Failed call to fitbit api {uri} , status code is {response.StatusCode} , and content is {response.Content}");
            }

        }



    }
}