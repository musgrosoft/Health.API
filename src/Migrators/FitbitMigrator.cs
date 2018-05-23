using System;
using System.Threading.Tasks;
using Services.Fitbit;
using Services.MyHealth;
using Utils;

namespace Migrators
{
    public class FitbitMigrator
    {
        private readonly ILogger _logger;
        private IHealthService _healthService;
        private IFitbitService _fitbitService;
        private readonly ICalendar _calendar;
        
        private const int FITBIT_HOURLY_RATE_LIMIT = 150;
        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_FITBIT_DATE = new DateTime(2017, 5, 1);

        public FitbitMigrator(IHealthService healthService, ILogger logger, IFitbitService fitbitService, ICalendar calendar)
        {
            _healthService = healthService;
            _logger = logger;
            _fitbitService = fitbitService;
            _calendar = calendar;
        }
        
        public async Task MigrateStepCounts()
        {
            var latestStepDate = _healthService.GetLatestStepCountDate(MIN_FITBIT_DATE);
            _logger.Log($"STEP COUNT : Latest Step record has a date of : {latestStepDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestStepDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"STEP COUNT : Retrieving Step records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var dailySteps = await _fitbitService.GetStepCounts(fromDate, _calendar.Now());

            _healthService.UpsertStepCounts(dailySteps);
        }
        
        public async Task MigrateActivitySummaries()
        {
            var latestActivityDate  = _healthService.GetLatestActivitySummaryDate(MIN_FITBIT_DATE);
            _logger.Log($"ACTIVITY SUMMARY : Latest Activity record has a date of : {latestActivityDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestActivityDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"ACTIVITY SUMMARY : Retrieving Activity records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {latestActivityDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var dailyActivites = await _fitbitService.GetDailyActivities(fromDate, _calendar.Now());

            await _healthService.UpsertDailyActivities(dailyActivites);
        }

        public async Task MigrateRestingHeartRates()
        {
            var latestRestingHeartRateDate = _healthService.GetLatestRestingHeartRateDate(MIN_FITBIT_DATE);
            _logger.Log($"RESTING HEART RATE : Latest Resting Heart Rate record has a date of : {latestRestingHeartRateDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestRestingHeartRateDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"RESTING HEART RATE : Retrieving Resting Heart Rate records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var restingHeartRates = await _fitbitService.GetRestingHeartRates(getDataFromDate, _calendar.Now());

            await _healthService.UpsertRestingHeartRates(restingHeartRates);

            await _healthService.AddMovingAveragesToRestingHeartRates();
        }

        public async Task MigrateHeartSummaries()
        {
            var latestHeartZonesDate = _healthService.GetLatestHeartSummaryDate(MIN_FITBIT_DATE);
            _logger.Log($"HEART SUMMARY : Latest Heart Zone Data has a date of : {latestHeartZonesDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestHeartZonesDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"HEART SUMMARY : Retrieving Heart Zone Data records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");
                
            var heartSummaries = await _fitbitService.GetHeartSummaries(getDataFromDate, _calendar.Now());

            await _healthService.UpsertDailyHeartSummaries(heartSummaries);
        }

        //        public async Task MigrateCalorieData()
        //        {
        //        }

    }
}
