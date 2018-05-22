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
        private IFitbitAggregator _fitbitAggregator;

        public FitbitService(IConfig config, ILogger logger, IFitbitAggregator fitbitAggregator)
        {
            _logger = logger;
            _config = config;
            _fitbitAggregator = fitbitAggregator;
        }

        public async Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate)
        {
            var fitbitDailyActivities = await _fitbitAggregator.GetFitbitDailyActivities(fromDate, toDate);

            return fitbitDailyActivities.Select(x => new StepCount
            {
                DateTime = x.DateTime,
                Count = x.summary.steps
            });
        }

        public async Task<IEnumerable<ActivitySummary>> GetDailyActivities(DateTime fromDate, DateTime toDate)
        {
            var fitbitDailyActivities = await _fitbitAggregator.GetFitbitDailyActivities(fromDate, toDate);

            return fitbitDailyActivities.Select(x => new ActivitySummary
            {
                DateTime = x.DateTime,
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
            var heartActivies = await _fitbitAggregator.GetFitbitHeartActivities(fromDate, toDate);

            return heartActivies
                    .Where(a => a.value.restingHeartRate != 0)
                    .Select(x => new RestingHeartRate
                    {
                        DateTime = x.dateTime,
                        Beats = x.value.restingHeartRate
                    });
        }
        
        public async Task<IEnumerable<HeartSummary>> GetHeartSummaries(DateTime fromDate, DateTime toDate)
        {
            var heartActivies = await _fitbitAggregator.GetFitbitHeartActivities(fromDate, toDate);

            return heartActivies.Select(x => new HeartSummary
            {
                DateTime = x.dateTime,
                OutOfRangeMinutes = x.value.heartRateZones.First(y => y.name == "Out of Range").minutes,
                FatBurnMinutes = x.value.heartRateZones.First(y => y.name == "Fat Burn").minutes,
                CardioMinutes = x.value.heartRateZones.First(y => y.name == "Cardio").minutes,
                PeakMinutes = x.value.heartRateZones.First(y => y.name == "Peak").minutes
            });
        }

    }
}