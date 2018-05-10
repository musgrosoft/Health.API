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
    public class FitbitClient : IFitbitClient
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

        

        

        private async Task<StepCount> GetStepCount(DateTime date)
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

            _logger.Log($"no stepcount found for date : {date}");

            return null;
        }

        public async Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate)
        {
            var stepCounts = new List<StepCount>();

            for (DateTime date = fromDate; date < toDate; date = date.AddDays(1))
            {
                var dailySteps = await GetStepCount(date);
                if (dailySteps != null)
                {
                    stepCounts.Add(dailySteps);
                }
            }

            return stepCounts;
        }

        public async Task<IEnumerable<DailyActivity>> GetDailyActivities(DateTime fromDate, DateTime toDate)
        {
            var dailyActivities = new List<DailyActivity>();

            for (DateTime date = fromDate; 
                date < toDate; 
                date = date.AddDays(1))
            {
                var dailyActivity = await GetDailyActivity(date);
                if (dailyActivity != null)
                {
                    dailyActivities.Add(dailyActivity);
                }
            }

            return dailyActivities;

        }

        private async Task<DailyActivity> GetDailyActivity(DateTime date)
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

        private async Task<IEnumerable<RestingHeartRate>> GetMonthOfRestingHeartRates(DateTime dateTime)
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

        public async Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate)
        {
            //for (DateTime dateTime = fromDate.AddMonths(1);
            //    dateTime < DateTime.Now.AddMonths(1).AddDays(1);
            //    dateTime = dateTime.AddMonths(1))
            //{

            //}

            var restingHeartRates = new List<RestingHeartRate>();

            for (DateTime dateTime = fromDate;
                dateTime < toDate;
                dateTime = dateTime.AddMonths(1))
            {
                var monthRestingHeartRates = await GetMonthOfRestingHeartRates(dateTime);

                if (monthRestingHeartRates != null)
                {
                    monthRestingHeartRates = restingHeartRates.Where(x => x.DateTime >= fromDate && x.DateTime <= toDate);

                    restingHeartRates.AddRange(monthRestingHeartRates);
                }

            }

            return restingHeartRates;

        }

        public async Task<IEnumerable<HeartRateZoneSummary>> GetMonthOfHeartZones(DateTime date)
        {

            var heartSummaries = await GetMonthOfHeartSummaries(date);


            var smallHeartRateSummaries = heartSummaries.activitiesHeart.Select(x => new HeartRateZoneSummary
            {
                DateTime = x.dateTime,
               // RestingHeartRate = x.value.restingHeartRate,
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