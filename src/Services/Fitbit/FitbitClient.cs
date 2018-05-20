//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using Newtonsoft.Json;
//using Services.Fitbit.Domain;
//using System.Threading.Tasks;
//using Repositories.Models;
//using Utils;


//namespace Services.Fitbit
//{
//    public class FitbitService : IFitbitService
//    {
        
//        private readonly ILogger _logger;
//        private readonly string _accessToken;
//        private readonly ICalendar _calendar;

//        private const string FITBIT_BASE_URL = "https://api.fitbit.com";
////        private static string theAccessToken;
//        private const int FITBIT_HOURLY_RATE_LIMIT = 150;

//        private IConfig _config { get; }
//        private HttpClient _httpClient;

//        public FitbitService(IConfig config, ILogger logger, string accessToken, ICalendar calendar, HttpClient httpClient)
//        {
        
//            _logger = logger;
//            _accessToken = accessToken;
//            _calendar = calendar;
//            _config = config;
//            _httpClient = httpClient;
//        }

        

        

//        private async Task<StepCount> GetStepCount(DateTime date)
//        {
//            var fitbitDailyActivity = await GetActivity(date);

//            if (fitbitDailyActivity != null)
//            {
//                return  new StepCount
//                {
//                    DateTime = date,
//                    Count = fitbitDailyActivity.summary.steps
//                };
//            }

//            _logger.Log($"no stepcount found for date : {date}");

//            return null;
//        }

//        public async Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate)
//        {
//            var stepCounts = new List<StepCount>();

//            for (DateTime date = fromDate; 
//                date < _calendar.Now(); 
//                date = date.AddDays(1))
//            {
//                var dailySteps = await GetStepCount(date);
//                if (dailySteps != null)
//                {
//                    stepCounts.Add(dailySteps);
//                }
//            }

//            return stepCounts;
//        }



//        public async Task<IEnumerable<DailyActivity>> GetDailyActivities(DateTime fromDate)
//        {
//            var dailyActivities = new List<DailyActivity>();

//            for (DateTime date = fromDate; 
//                date < _calendar.Now(); 
//                date = date.AddDays(1))
//            {
//                var dailyActivity = await GetDailyActivity(date);
//                if (dailyActivity != null)
//                {
//                    dailyActivities.Add(dailyActivity);
//                }
//            }

//            return dailyActivities;

//        }

//        private async Task<DailyActivity> GetDailyActivity(DateTime date)
//        {
//            var fitbitDailyActivity = await GetActivity(date);

//            if (fitbitDailyActivity != null)
//            {
//                return new DailyActivity
//                {
//                    DateTime = date,
//                    //activityCalories
//                    //caloriesBMR
//                    //caloriesOut
//                    //distances
//                    //elevation
//                    FairlyActiveMinutes = fitbitDailyActivity.summary.fairlyActiveMinutes,
//                    //floors
//                    LightlyActiveMinutes = fitbitDailyActivity.summary.lightlyActiveMinutes,
//                    //marginalCalories
//                    SedentaryMinutes = fitbitDailyActivity.summary.sedentaryMinutes,
//                    VeryActiveMinutes = fitbitDailyActivity.summary.veryActiveMinutes
//                };
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private async Task<FitbitDailyActivity> GetActivity(DateTime date)
//        {
//            var uri = FITBIT_BASE_URL + $"/1/user/{_config.FitbitUserId}/activities/date/{date:yyyy-MM-dd}.json";
//            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

//            var response = await _httpClient.GetAsync(uri);
//            if (response.IsSuccessStatusCode)
//            {

//                var content = await response.Content.ReadAsStringAsync();

//                var data = JsonConvert.DeserializeObject<FitbitDailyActivity>(content);

//                return data;
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private async Task<IEnumerable<RestingHeartRate>> GetMonthOfRestingHeartRates(DateTime dateTime)
//        {
//            var heartSummaries = await GetMonthOfHeartSummaries(dateTime);
//            //might need to check if bpm is zero
//            var restingHeartRates = heartSummaries.activitiesHeart.Where(a=>a.value.restingHeartRate != 0).Select(x => new RestingHeartRate
//            {
//                DateTime = x.dateTime,
//                Beats = x.value.restingHeartRate
//            });

//            return restingHeartRates;
//        }

//        public async Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate)
//        {
//            var restingHeartRates = new List<RestingHeartRate>();

//            for (DateTime dateTime = fromDate.AddMonths(1);
//                dateTime < _calendar.Now().AddMonths(1).AddDays(1);
//                dateTime = dateTime.AddMonths(1))
//            {
//                var monthRestingHeartRates = await GetMonthOfRestingHeartRates(dateTime);

//                if (monthRestingHeartRates != null)
//                {
//                    monthRestingHeartRates = restingHeartRates.Where(x => x.DateTime >= fromDate && x.DateTime <= _calendar.Now());

//                    restingHeartRates.AddRange(monthRestingHeartRates);
//                }

//            }

//            return restingHeartRates;

//        }

//        private async Task<IEnumerable<HeartRateZoneSummary>> GetMonthOfHeartZones(DateTime date)
//        {

//            var heartSummaries = await GetMonthOfHeartSummaries(date);


//            var smallHeartRateSummaries = heartSummaries.activitiesHeart.Select(x => new HeartRateZoneSummary
//            {
//                DateTime = x.dateTime,
//               // RestingHeartRate = x.value.restingHeartRate,
//                OutOfRangeMinutes = x.value.heartRateZones.First(y => y.name == "Out of Range").minutes,
//                FatBurnMinutes = x.value.heartRateZones.First(y => y.name == "Fat Burn").minutes,
//                CardioMinutes = x.value.heartRateZones.First(y => y.name == "Cardio").minutes,
//                PeakMinutes = x.value.heartRateZones.First(y => y.name == "Peak").minutes
//            });
            

//            return smallHeartRateSummaries;
//        }

//        public async Task<IEnumerable<HeartRateZoneSummary>> GetHeartZones(DateTime fromDate)
//        {
//            var heartSummaries = new List<HeartRateZoneSummary>();

//            for (DateTime date = fromDate.AddMonths(1);
//                date < _calendar.Now().AddMonths(1).AddDays(1);
//                date = date.AddMonths(1))
//            {
//                var monthHeartSummaries = await GetMonthOfHeartZones(date);

//                if (monthHeartSummaries != null)
//                {
//                    monthHeartSummaries = monthHeartSummaries.Where(x => x.DateTime >= fromDate && x.DateTime <= _calendar.Now());

//                    heartSummaries.AddRange(monthHeartSummaries);
//                }
//            }

//            return heartSummaries;
//        }

        



//    }
//}