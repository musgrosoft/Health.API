using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;
using Utils;

namespace Services.Fitbit
{
    public class FitbitService : IFitbitService
    {
        private readonly ILogger _logger;

        private const int FITBIT_HOURLY_RATE_LIMIT = 150;

        private IConfig _config { get; }
        private readonly IFitbitClientAggregator _fitbitClientAggregator;
        private readonly IFitbitClient _fitbitClient;
        private readonly IFitbitAuthenticator _fitbitAuthenticator;

        public FitbitService(IConfig config, ILogger logger, IFitbitClientAggregator fitbitClientAggregator, IFitbitClient fitbitClient, IFitbitAuthenticator fitbitAuthenticator)
        {
            _logger = logger;
            _config = config;
            _fitbitClientAggregator = fitbitClientAggregator;
            _fitbitClient = fitbitClient;
            _fitbitAuthenticator = fitbitAuthenticator;
        }

        public async Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate)
        {
            var fitbitDailyActivities = await _fitbitClientAggregator.GetFitbitDailyActivities(fromDate, toDate);

            return fitbitDailyActivities.Select(x => new StepCount
            {
                CreatedDate = x.DateTime,
                Count = x.summary.steps
            });
        }

        public async Task<IEnumerable<ActivitySummary>> GetActivitySummaries(DateTime fromDate, DateTime toDate)
        {
            var fitbitDailyActivities = await _fitbitClientAggregator.GetFitbitDailyActivities(fromDate, toDate);

            return fitbitDailyActivities.Select(x => new ActivitySummary
            {
                CreatedDate = x.DateTime,
                //activityCalories
                //caloriesBMR
                //caloriesOut
                //distances
                //elevation
                FairlyActiveMinutes = x.summary.fairlyActiveMinutes,
                //floors
                LightlyActiveMinutes = x.summary.lightlyActiveMinutes,
                //marginalCalories
                SedentaryMinutes = x.summary.sedentaryMinutes,
                VeryActiveMinutes = x.summary.veryActiveMinutes
            });

        }
        
        public async Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate)
        {
            var heartActivies = await _fitbitClientAggregator.GetFitbitHeartActivities(fromDate, toDate);

            return heartActivies
                    .Where(a => a.value.restingHeartRate != 0)
                    .Select(x => new RestingHeartRate
                    {
                        CreatedDate = x.dateTime,
                        Beats = x.value.restingHeartRate
                    });
        }

        public void Subscribe()
        {
            _fitbitClient.Subscribe();
        }

        public async Task SetTokens(string code)
        {
            await _fitbitAuthenticator.SetTokens(code);
        }

        public async Task<IEnumerable<HeartRateSummary>> GetHeartSummaries(DateTime fromDate, DateTime toDate)
        {
            var heartActivies = await _fitbitClientAggregator.GetFitbitHeartActivities(fromDate, toDate);

            return heartActivies.Select(x => new HeartRateSummary
            {
                CreatedDate = x.dateTime,
                OutOfRangeMinutes = x.value.heartRateZones.First(y => y.name == "Out of Range").minutes,
                FatBurnMinutes = x.value.heartRateZones.First(y => y.name == "Fat Burn").minutes,
                CardioMinutes = x.value.heartRateZones.First(y => y.name == "Cardio").minutes,
                PeakMinutes = x.value.heartRateZones.First(y => y.name == "Peak").minutes
            });
        }

    }
}