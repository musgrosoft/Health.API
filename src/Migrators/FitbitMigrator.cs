using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Services.Fitbit;
using Services.MyHealth;

namespace Migrators
{
    public class FitbitMigrator
    {
        private readonly ILambdaLogger _logger;
        private HealthService _healthService;
        private FitbitClient _fitbitClient;

        private DateTime MIN_FITBIT_DATE = new DateTime(2017, 5, 1);


        private const int FITBIT_HOURLY_RATE_LIMIT = 150;
        private const int SEARCH_DAYS_PREVIOUS = 10;

        public FitbitMigrator(ILambdaLogger logger)
        {
            _logger = logger;
        }


        public FitbitMigrator(HealthService healthService, ILambdaLogger logger, FitbitClient fitbitClient)
        {
            _healthService = healthService;
            _logger = logger;
            _fitbitClient = fitbitClient;
        }


        public async Task MigrateStepData()
        {
            var latestStep = await _healthService.GetLatestStepData();
            var latestStepDate = latestStep?.DateTime ?? MIN_FITBIT_DATE;

            _logger.Log($"Latest Step record has a date of : {latestStepDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestStepDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"Retrieving Step records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            for (DateTime date = getDataFromDate; date < DateTime.Now && date < getDataFromDate.AddDays(FITBIT_HOURLY_RATE_LIMIT); date = date.AddDays(1))
            {
                var dailySteps = await _fitbitClient.GetDailySteps(date);

                if (dailySteps != null)
                {
                    _logger.Log($"Saving Step Data for {date:dd-MMM-yyyy HH:mm:ss (ddd)} : {dailySteps.Steps} steps");
                    await _healthService.SaveStepCount(dailySteps);

                }
                else
                {

                }
            }
        }


        public async Task MigrateActivity()
        {
            var latestActivity = await _healthService.GetLatestDailyActivity();
            var latestActivityDate = latestActivity?.DateTime ?? MIN_FITBIT_DATE;

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
                    await _healthService.SaveDailyActivity(dailyActivity);
                }
            }
        }

        public async Task MigrateRestingHeartRateData()
        {
            var latestRestingHeartRate = await _healthService.GetLatestRestingHeartRate();
            var latestRestingHeartRateDate = latestRestingHeartRate?.DateTime ?? MIN_FITBIT_DATE;

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
                        await _healthService.SaveRestingHeartRate(restingHeartRate);
                    }
                }
            }
        }

        public async Task MigrateHeartZoneData()
        {
            //var latestHeartZonesDate = new DateTime(2017, 12, 1);
            try
            {
               var latestHeartZones = await _healthService.GetLatestHeartRateDailySummary() ;
                var latestHeartZonesDate = latestHeartZones?.DateTime ?? MIN_FITBIT_DATE;
             

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

                        foreach (var smallHeartRateSummary in heartSummaries)
                        {
                            //if (activityHeart.restingHeartRate != 0)
                            //{
                            //var smallHeartRateSummary = new SmallHeartRateSummary
                            //{
                            //    DateTime = DateTime.Parse(activityHeart.DateTime),
                            //    RestingHeartRate = activityHeart.value.restingHeartRate,
                            //    OutOfRangeMinutes = activityHeart.value.heartRateZones.First(x => x.name == "Out of Range").minutes,
                            //    FatBurnMinutes = activityHeart.value.heartRateZones.First(x => x.name == "Fat Burn").minutes,
                            //    CardioMinutes = activityHeart.value.heartRateZones.First(x => x.name == "Cardio").minutes,
                            //    PeakMinutes = activityHeart.value.heartRateZones.First(x => x.name == "Peak").minutes
                            //};

                            if (smallHeartRateSummary.RestingHeartRate != 0)
                            {
                                _logger.Log($"Saving heart rate summary for {smallHeartRateSummary.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)}, {smallHeartRateSummary.RestingHeartRate}");
                                await _healthService.SaveFitbitDailyHeartSummaryDataAsync(smallHeartRateSummary);
                            }
                            //}
                            else
                            {
                                _logger.Log($"No resting hear rate data found for {smallHeartRateSummary.DateTime}");
                            }
                        }
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
