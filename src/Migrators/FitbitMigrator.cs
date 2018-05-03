using System;
using System.Linq;
using System.Threading.Tasks;
using Services.Fitbit;
using Services.MyHealth;
using Utils;

namespace Migrators
{
    public class FitbitMigrator
    {
        private readonly ILogger _logger;
        private HealthService _healthService;
        private FitbitClient _fitbitClient;
        

        private const int FITBIT_HOURLY_RATE_LIMIT = 150;
        private const int SEARCH_DAYS_PREVIOUS = 10;

        public FitbitMigrator(ILogger logger)
        {
            _logger = logger;
        }
        
        public FitbitMigrator(HealthService healthService, ILogger logger, FitbitClient fitbitClient)
        {
            _healthService = healthService;
            _logger = logger;
            _fitbitClient = fitbitClient;
        }
        
        public async Task MigrateStepData()
        {
            var latestStepDate = _healthService.GetLatestStepCountDate();

            _logger.Log($"Latest Step record has a date of : {latestStepDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestStepDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"Retrieving Step records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            for (DateTime date = getDataFromDate; date < DateTime.Now && date < getDataFromDate.AddDays(FITBIT_HOURLY_RATE_LIMIT); date = date.AddDays(1))
            {
                var dailySteps = await _fitbitClient.GetDailySteps(date);

                if (dailySteps != null)
                {
                    _logger.Log($"Saving Step Data for {date:dd-MMM-yyyy HH:mm:ss (ddd)} : {dailySteps.Count} steps");
                    await _healthService.UpsertStepCount(dailySteps);

                }
                else
                {

                }
            }
        }


        public async Task MigrateActivity()
        {
            var latestActivityDate  = _healthService.GetLatestDailyActivityDate();

            _logger.Log($"Latest Activity record has a date of : {latestActivityDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            latestActivityDate = latestActivityDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"Retrieving Activity records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {latestActivityDate:dd-MMM-yyyy HH:mm:ss (ddd)}");
            
            //for (DateTime date = DateTime.Now; date > DateTime.Now.AddDays(-100); date = date.AddDays(-1))
            for (DateTime date = latestActivityDate; date < DateTime.Now && date < latestActivityDate.AddDays(FITBIT_HOURLY_RATE_LIMIT); date = date.AddDays(1))
            {
                var dailyActivity = await _fitbitClient.GetDailyActivity(date);

                if (dailyActivity != null)
                {
                    _logger.Log($"Saving Activity Data for {date:dd-MMM-yyyy HH:mm:ss (ddd)} : {dailyActivity.SedentaryMinutes} sedentary minutes, {dailyActivity.LightlyActiveMinutes} lightly active minutes, {dailyActivity.FairlyActiveMinutes} fairly active minutes, {dailyActivity.VeryActiveMinutes} very active minutes.");
                    await _healthService.UpsertDailyActivity(dailyActivity);
                }
            }
        }

        public async Task MigrateRestingHeartRateData()
        {
            var latestRestingHeartRateDate = _healthService.GetLatestRestingHeartRateDate();

            _logger.Log($"Latest Resting Heart Rate record has a date of : {latestRestingHeartRateDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestRestingHeartRateDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            _logger.Log($"Retrieving Resting Heart Rate records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            for (DateTime dateTime = getDataFromDate.AddMonths(1);
                dateTime < DateTime.Now.AddMonths(1).AddDays(1);
                dateTime = dateTime.AddMonths(1))
            {
                var restingHeartRates = await _fitbitClient.GetMonthOfRestingHeartRates(dateTime);

                if (restingHeartRates != null)
                {
                    restingHeartRates = restingHeartRates.Where(x => x.DateTime >= getDataFromDate && x.DateTime <= DateTime.Now);

                    foreach (var restingHeartRate in restingHeartRates)
                    {
                        _logger.Log($"About to save Resting Heart Rate record : {restingHeartRate.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {restingHeartRate.Beats} beats");
                        await _healthService.UpsertRestingHeartRate(restingHeartRate);
                    }
                }
            }

            await _healthService.AddMovingAveragesToRestingHeartRates();
        }

        public async Task MigrateHeartZoneData()
        {
            try
            {
               var latestHeartZonesDate = _healthService.GetLatestHeartRateDailySummaryDate();

                _logger.Log(
                    $"Latest Heart Zone Data has a date of : {latestHeartZonesDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

                var getDataFromDate = latestHeartZonesDate.AddDays(-SEARCH_DAYS_PREVIOUS);
                _logger.Log(
                    $"Retrieving Heart Zone Data records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

                //get min date
                for (DateTime date = getDataFromDate.AddMonths(1);
                    date < DateTime.Now.AddMonths(1).AddDays(1);
                    date = date.AddMonths(1))
                {
                    var heartSummaries = await _fitbitClient.GetMonthOfHeartZones(date);
             
                    if (heartSummaries != null)
                    {
                        heartSummaries = heartSummaries.Where(x => x.DateTime >= getDataFromDate && x.DateTime <= DateTime.Now);

                        _logger.Log($"Found {heartSummaries.Count()} Heart Zone Data records.");

                        await _healthService.UpsertDailyHeartSummaries(heartSummaries);
                    }
                }
                _logger.Log($"Finished that");
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }
        }

        //        public async Task MigrateCalorieData()
        //        {
        //            var latestActivityDate = (await _healthRepository.GetLatestCalorieDate()).AddDays(-10);
        //
        //            //for (DateTime date = DateTime.Now; date > DateTime.Now.AddDays(-100); date = date.AddDays(-1))
        //            for (DateTime date = latestActivityDate; date < DateTime.Now && date < latestActivityDate.AddDays(FITBIT_HOURLY_RATE_LIMIT); date = date.AddDays(1))
        //            {
        //                var fitbitDailyActivity = await _fitbitClient.GetActivity(date);
        //
        //                if (fitbitDailyActivity != null)
        //                {
        //                    fitbitDailyActivity.summary.theDate = date.ToString("yyyy-MM-dd");
        //
        //                    var data = new DailyActivitySummaryData
        //                    {
        //                        theDate = fitbitDailyActivity.summary.theDate,
        //                        //activityCalories
        //                        //caloriesBMR
        //                        //caloriesOut
        //                        //distances
        //                        //elevation
        //                        fairlyActiveMinutes = fitbitDailyActivity.summary.fairlyActiveMinutes,
        //                        //floors
        //                        lightlyActiveMinutes = fitbitDailyActivity.summary.lightlyActiveMinutes,
        //                        //marginalCalories
        //                        sedentaryMinutes = fitbitDailyActivity.summary.sedentaryMinutes,
        //                        steps = fitbitDailyActivity.summary.steps,
        //                        veryActiveMinutes = fitbitDailyActivity.summary.veryActiveMinutes
        //                    };
        //
        //                    await _healthRepository.SaveFitbitDailyActivitySummaryData(data, date);
        //                }
        //            }
        //        }

    }
}
