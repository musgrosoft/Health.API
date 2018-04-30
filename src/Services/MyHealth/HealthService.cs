using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Services.MyHealth.Domain;
using Utils;

namespace Services.MyHealth
{
    public class HealthService
    {   
        private readonly IConfig _config;
        private readonly ILogger _logger;
        private const string HEALTH_API_BASE_URL = "http://musgrosoft-health-api.azurewebsites.net";
        
        public HealthService(IConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<Weight> GetLatestWeight()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/Weights?$top=1&$orderby=DateTime%20desc";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var weightsJson = await response.Content.ReadAsStringAsync();
                var weights = JsonConvert.DeserializeObject<ODataResponse<Weight>>(weightsJson);
                
                return weights.value.FirstOrDefault();
            }
            else
            {
                throw new Exception($"non ok status code : {path}, status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }
        

        public async Task<BloodPressure> GetLatestBloodPressure()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/BloodPressures?$top=1&$orderby=DateTime%20desc";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var bpsJson = await response.Content.ReadAsStringAsync();
                var bps = JsonConvert.DeserializeObject<ODataResponse<BloodPressure>>(bpsJson);
                
                return bps.value.FirstOrDefault();

            }
            else
            {
                throw new Exception($"non ok status code : {path}, status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task<DailySteps> GetLatestStepData()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/Steps";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var dailyStepsJson = await response.Content.ReadAsStringAsync();
                var dailySteps = JsonConvert.DeserializeObject<List<DailySteps>>(dailyStepsJson);

                if (!dailySteps.Any())
                {
                    return null;
                }

                var lastDailyStep = dailySteps.OrderBy(x => x.DateTime).Last();

                return lastDailyStep;
            }
            else
            {
                throw new Exception($"non ok status code : {path}, status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }
        
        public async Task<Activity> GetLatestDailyActivity()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/DailyActivities";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var activityJson = await response.Content.ReadAsStringAsync();
                var activities = JsonConvert.DeserializeObject<List<Activity>>(activityJson);

                if (activities == null || !activities.Any())
                {
                    return null;
                }

                var lastActivity = activities.OrderBy(x => x.DateTime).Last();

                return lastActivity;
            }
            else
            {
                throw new Exception($"non ok status code : {path}, status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task<RestingHeartRate> GetLatestRestingHeartRate()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/RestingHeartRates";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var heartsJson = await response.Content.ReadAsStringAsync();
                var hearts = JsonConvert.DeserializeObject<List<RestingHeartRate>>(heartsJson);

                if (!hearts.Any())
                {
                    return null;
                }

                var lastHeart = hearts.OrderBy(x => x.DateTime).Last();

                return lastHeart;
            }
            else
            {
                throw new Exception($"non ok status code : {path}, status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task<SmallHeartRateSummary> GetLatestHeartRateDailySummary()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/HeartRateDailySummaries";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var heartRateSummaryJson = await response.Content.ReadAsStringAsync();
                var heartRateSummary = JsonConvert.DeserializeObject<List<SmallHeartRateSummary>>(heartRateSummaryJson);

                if (!heartRateSummary.Any())
                {
                    return null;
                }

                var lastHeartRateSummary = heartRateSummary.OrderBy(x => x.DateTime).Last();

                return lastHeartRateSummary;
            }
            else
            {
                throw new Exception($"non ok status code : {path}, status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }


        public async Task SaveWeight(Weight weight)
        {
            var path = $"{HEALTH_API_BASE_URL}/api/Weights";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(weight), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error saving weight non ok status code : {path} , status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task AddMovingAveragesToWeights()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/Weights/AddMovingAverages";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent("", Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error AddMovingAveragesToWeights non ok status code : {path} , status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task SaveBloodPressure(BloodPressure bloodPressure)
        {
            var path = $"{HEALTH_API_BASE_URL}/api/BloodPressures";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(bloodPressure), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error saving blood pressure non ok status code : {path} , status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task AddMovingAveragesToBloodPressures()
        {
            var path = $"{HEALTH_API_BASE_URL}/api/BloodPressures/AddMovingAverages";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent("", Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error AddMovingAveragesToBloodPressures non ok status code : {path} , status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task SaveStepCount(DailySteps dailySteps)
        {
            var path = $"{HEALTH_API_BASE_URL}/api/Steps";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(dailySteps), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error saving steps non ok status code : {path} , stsus code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task SaveDailyActivity(DailyActivity dailyActivity)
        {
            var path = $"{HEALTH_API_BASE_URL}/api/DailyActivities";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(dailyActivity), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error saving activity non ok status code : {path} , stsus code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task SaveRestingHeartRate(RestingHeartRate restingHeartRate)
        {
            var path = $"{HEALTH_API_BASE_URL}/api/RestingHeartRates";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(restingHeartRate), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error saving resting heart rate non ok status code : {path} , status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }
        }

        public async Task SaveFitbitDailyHeartSummaryDataAsync(SmallHeartRateSummary dailyHeartSummaryData)
        {
            var path = $"{HEALTH_API_BASE_URL}/api/HeartRateDailySummaries";
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(dailyHeartSummaryData), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error saving FitbitDailyHeartSummary non ok status code : {path} , status code is {response.StatusCode} , content is {await response.Content.ReadAsStringAsync()} ");
            }

        }






    }
}